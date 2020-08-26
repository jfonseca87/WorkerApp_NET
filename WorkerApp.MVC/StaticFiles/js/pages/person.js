// Globals
let fileModal
let modalDelete
let btnAddFile
let btnChangePhoto
let btnSaveProfile
let btnSaveFile
let btnDeleteFile
let selFileTypes
let inputFile
let progressBar
let textStatus
let filesAttachedTable
let parameters
let isPhotoFile = false
let deleteId = 0

$(document).ready(function () {
    parameters = getParameters(window.location.search)
    setDomObjects()

    btnSaveProfile.click(function () {
        const person = createPersonObject();
        savePersonData(person)
    });

    btnAddFile.bind('click', showAddFile)

    btnSaveFile.click(function () {
        saveFile()
    })

    btnDeleteFile.click(function () {
        deleteFile();
    })

    fileModal.on('show.bs.modal', function (e) {
        getFileTypes()
    })

    fileModal.on('hide.bs.modal', function (e) {
        inputFile[0].value = ''
        isPhotoFile = false
        cleanModalMessage()
    })

    if (parameters !== null) {
        getPerson()
    } else {
        createFilesAttachedTable([], getColumns())
        $('input[name=Email]').val(localStorage.getItem('userEmail'))
        hideLoading()
    }
})

function setDomObjects() {
    fileModal = $('#addFilesModal')
    modalDelete = $('#deleteModal')
    btnAddFile = $('#btnAddFile')
    btnChangePhoto = $('#btnChangePhoto')
    btnSaveProfile = $('#btnSaveProfile')
    btnSaveFile = $('#btnSaveFile') 
    btnDeleteFile = $('#btnDelete')
    selFileTypes = $('#fileType')
    inputFile = $('#file')
    progressBar = $('.progress-bar')
    textStatus = $('#text-status')
}

function createFilesAttachedTable(data, _columns) {
    filesAttachedTable = new Tabulator("#files-attached-table", {
        data: data,
        height: "450px",
        layout: "fitColumns",
        movableColumns: true,
        columns: _columns
    })
}

const addDownloadButton = (cell) => {
    return `<button class="btn btn-success" onclick="downloadFile('${cell._cell.value}')" title="Download"><i class="fas fa-cloud-download-alt"></i></button>`
}

const addDeleteButton = (cell) => {
    return `<button class="btn btn-danger" onclick="showDeletePopup('${cell._cell.value}')" title="Delete"><i class="fas fa-trash-alt"></i></button>`
}

function filterFileTypesData(data) {
    const filesAttached = filesAttachedTable.options.data
    let filteredFilesTypes = data

    for (fileAttached of filesAttached) {
        filteredFilesTypes = filteredFilesTypes.filter(x => x.FileType !== fileAttached.FileType)
    }

    return filteredFilesTypes
}

function processSelectOptions(data) {
    const options = data.map(option => {
        return `<option value="${option.FileId}"> ${option.FileType} </option>`
    })

    return options
}

function fillSelectOptions(options) {
    const defaultValue = `<option value=""> Select... </option>`
    selFileTypes.html('')
    selFileTypes.append(defaultValue)
    selFileTypes.append(options)
}

function createPersonObject() {
    const personObj = {}
    const inputs = $('input[type=text]')

    for (input of inputs) {
        personObj[input.name] = input.value
    }

    personObj.PersonId = $('input[name=PersonId]').val()

    if (localStorage.getItem('userId') !== null) {
        personObj.UserId = localStorage.getItem('userId')
    }
    
    return personObj
}

function enableFilesTab() {
    $('#profile-tab').removeClass('disabled')
}

function showAddFile(control) {
    if (control.target.id === 'btnChangePhoto') {
        $('.fileType').hide()
        isPhotoFile = true
    } else {
        $('.fileType').show()
    }

    fileModal.modal('show')
}

function showDeletePopup(id) {
    deleteId = id
    modalDelete.modal('show')
}

function downloadFile(path) {
    window.open(path)
}

function enableChangePhotoButton() {
    btnChangePhoto.removeAttr('disabled')
    btnChangePhoto.bind('click', showAddFile)
}

function setParameter(id) {
    document.location.search = `?id=${id}`
}

function setProgressBarValue(value) {
    const percentValue = `${value}%`
    const status = value === 100 ? 'Complete' : 'Incomplete'
    
    progressBar[0].style.width = percentValue
    progressBar.html(percentValue)
    textStatus.html(status)
}

function setValues(data) {
    for (property in data) {
        $(`input[name=${property}]`).val(data[property])
    }

    if (data.Photo !== null) {
        $('.img-person').attr('src', data.Photo)
    }
}

function setFileObject() {
    const fileType = isPhotoFile === true ? 'Photo' : selFileTypes[0].value 
    var objFile = new FormData()
    objFile.append('FileType', fileType)
    objFile.append('File', inputFile[0].files[0])
    objFile.append('PersonId', $('input[name=PersonId]').val())

    return objFile
}

function getColumns() {
   return  [
        { title: "File Type", field: "FileType", width: 400 },
        { title: "File", field: "Path", headerSort: false, formatter: addDownloadButton, width: 80 },
        { title: "", field: "FileAttachedId", headerSort: false, formatter: addDeleteButton, width: 80 }
    ]
}

function getColumnsReadOnly() {
    return [
        { title: "File Type", field: "FileType", width: 400 },
        { title: "File", field: "Path", headerSort: false, formatter: addDownloadButton, width: 80 }
    ]
}

function readonlyMode() {
    $('button').remove()
    $('input[type=text]').attr('readonly', true)
}

function getPerson() {
    const id = parameters['id']

    $.ajax({
        method: 'GET',
        url: `/Person/GetPerson/${id}`,
        success: function (data) {
            if (data.Message === 'PersonNotFound') {
                showAlert('warning', 'Person not found')
                return
            }

            setValues(data)
            enableFilesTab()
            enableChangePhotoButton()
            getProfileStatus()

            if (parameters['read']) {
                createFilesAttachedTable(data.FilesAttached, getColumnsReadOnly())
                readonlyMode()
            } else {
                createFilesAttachedTable(data.FilesAttached, getColumns())
            }

            hideLoading()
        },
        error: function () {
            console.log('Error')
            showAlert('danger', 'An error has ocurred, please contact with the system administrator')
        }
    })
}

function getFileTypes() {
    $.ajax({
        method: 'GET',
        url: `/File/GetFiles`,
        success: function (data) {
            const filteredData = filterFileTypesData(data)
            const optionsHtml = processSelectOptions(filteredData)
            fillSelectOptions(optionsHtml)
        },
        error: function () {
            console.log('Error')
            showAlert('danger', 'An error has ocurred, please contact with the system administrator')
        }
    })
}

function getProfileStatus() {
    const personId = $('input[name=PersonId]').val()

    $.ajax({
        method: 'GET',
        url: `/Person/GetProfileStatus/${personId}`,
        success: function (data) {
            if (data.Message === 'PersonNotFound') {
                showAlert('warning', 'Person not found')
                return
            }

            setProgressBarValue(data.Message);
        },
        error: function () {
            console.log('Error')
            showAlert('danger', 'An error has ocurred, please contact with the system administrator')
        }
    })
}

function savePersonData(data) {
    $.ajax({
        method: 'POST',
        url: `/Person/Person`,
        data: data,
        success: function (data) {
            switch (data.Message) {
                case 'Created':
                    clearLocalStorage()
                    setParameter(data.PersonId)
                    $('input[name=personId]').val(data.PersonId)
                    showAlert('success', 'The person has been successfully created')
                    break;
                case 'Updated':
                    showAlert('success', 'The person has been successfully updated')
                    break;
                case 'Error':
                    let errors = createErrorMessageValidation(data.Errors)
                    showAlert('danger', errors)
                    break;
                default:
                    break;
            }
        },
        fail: function () {
            console.log('Error')
            showAlert('danger', 'An error has ocurred, please contact with the system administrator')
        }
    })
}

function saveFile() {
    const objFile = setFileObject()

    $.ajax({
        method: 'POST',
        url: `/FileAttached/FileAdd`,
        data: objFile,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.Message === 'Error') {
                let errors = createErrorMessageValidation(data.Errors)
                showAlert('danger-modal', errors)
            } else {
                if (isPhotoFile) {
                    $('.img-person').attr('src', data.Message)
                    getProfileStatus()
                } else {
                    getPerson()
                }

                fileModal.modal('hide')
            } 
        },
        fail: function () {
            console.log('Error')
            showAlert('danger', 'An error has ocurred, please contact with the system administrator')
        }
    })
}

function deleteFile() {
    const personId = $('input[name=PersonId]').val()

    $.ajax({
        method: 'GET',
        url: `/FileAttached/DeleteFile?personId=${personId}&fileId=${deleteId}`,
        success: function (data) {
            modalDelete.modal('hide')
            getPerson()
            showAlert('success', 'The file has been successfully deleted')
        },
        fail: function () {
            console.log('Error')
            showAlert('danger', 'An error has ocurred, please contact with the system administrator')
        }
    })
}

function hideLoading() {
    $('.loading').hide()
}

function showLoading() {
    $('.loading').show()
}