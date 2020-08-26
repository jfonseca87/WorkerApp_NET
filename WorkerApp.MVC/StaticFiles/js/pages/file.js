let noDataContainer
let dataContainer
let tableBodyContainer
let btnNew
let btnSave
let btnDelete
let modalForm
let modalDelete
let deleteId
let editMode = false

$(document).ready(function () {
    setDomInVariables()

    btnNew.click(function () {
        editMode = false
        modalForm.modal('show')
    });

    btnSave.click(function () {
        const objFile = getDataByForm();
        saveFile(objFile)
    }) 

    btnDelete.click(function () {
        deleteFile()
    })

    modalForm.on('show.bs.modal', function (e) {
        cleanForm()
    })

    modalForm.on('hide.bs.modal', function (e) {
        getFiles()
    })

    modalDelete.on('hide.bs.modal', function (e) {
        getFiles()
    })

    getFiles()
});

function createTable(data) {
    new Tabulator("#files-table", {
        data: data,
        height: "600px",
        layout: "fitColumns",
        pagination: "local",
        paginationSize: 10,
        movableColumns: true,
        columns: [
            { title: "File Type", field: "FileType", width: 200 },
            { title: "Allowed Extensions", field: "AllowedExtensions", width: 750 },
            { title: "", field: "FileId", headerSort:false , formatter: addButtons }
        ],
    });
}

const addButtons = function addButtons(cell) {
    return `<button class="btn btn-warning" onclick="getFile('${cell._cell.value}')" title="Edit"><i class="fas fa-edit"></i></button>
     <button class="btn btn-danger" onclick="showDeletePopup('${cell._cell.value}')" title="Delete"><i class="fas fa-trash-alt"></i></button>`
}

function setDomInVariables() {
    noDataContainer = $('.no-data')
    dataContainer = $('.file-data')
    tableBodyContainer = $('#file-table-body')
    btnNew = $('.new-file')
    btnSave = $('#btnSave')
    btnDelete = $('#btnDelete')
    modalForm = $('#formModal')
    modalDelete = $('#deleteModal')
}

function getDataByForm() {
    const objForm = {}
    const inputs = $('input[type="text"]')

    objForm['FileId'] = $('input[type="hidden"]')[0].value

    for (input of inputs) {
        objForm[input.name] = input.value
    }

    const textAreas = $('textarea')

    for (textArea of textAreas) {
        objForm[textArea.name] = textArea.value
    }

    return objForm
}

function setDataForm(data) {
    editMode = true
    $('input[type="hidden"]').val(data['FileId'])

    for (prop in data) {
        if (prop !== 'FileId') {
            $(`[name='${prop}']`).val(data[prop])
        }
    }
}

function cleanForm() {
    const inputs = $('input[type="text"]')
    for (input of inputs) {
        input.value = ''
    }

    const textAreas = $('textarea')
    for (textArea of textAreas) {
        textArea.value = ''
    }

    $('.danger-container-modal').html('');
}

function showDeletePopup(id) {
    deleteId = id
    modalDelete.modal('show')
}

function getFiles() {
    $.ajax({
        method: 'GET',
        url: '/File/GetFiles',
        success: function (data) {
            if (data.length > 0) {                
                createTable(data)
                dataContainer.show()
            } else {
                noDataContainer.show()
                dataContainer.hide()
            }
        },
        fail: function () {
            cleanMessages()
            console.log('Error')
            showAlert('danger', 'An error has ocurred, please contact with the systems administrator')
        }
    })
}

function getFile(id) {
    $.ajax({
        method: 'GET',
        url: `/File/GetFile/${id}`,
        success: function (data) {
            modalForm.modal('show')
            setDataForm(data);
        },
        fail: function () {
            cleanMessages()
            console.log('Error')
            showAlert('danger', 'An error has ocurred, please contact with the systems administrator')
        }
    })
}

function saveFile(objForm) {
    $.ajax({
        method: 'POST',
        url: '/File/MaintenanceFile',
        data: objForm,
        success: function (data) {
            switch (data.Message) {
                case 'Created':
                    showAlert('success', 'The record has been successfully created')
                    modalForm.modal('hide')
                    break;
                case 'Updated':
                    showAlert('success', 'The record has been successfully updated')
                    modalForm.modal('hide')
                    break;
                case 'Error':
                    let errors = createErrorMessageValidation(data.Errors)
                    showAlert('danger-modal', errors)
                    break;
                default:
                    break;
            }
        },
        fail: function () {
            cleanMessages()
            console.log('Error')
            showAlert('danger', 'An error has ocurred, please contact with the systems administrator')
        }
    })
}

function deleteFile() {
    $.ajax({
        method: 'POST',
        url: `/File/DeleteFile/${deleteId}`,
        success: function (data) {
            switch (data.Result) {
                case 'Success':
                    showAlert('success', 'The record has been successfully deleted')
                    modalDelete.modal('hide')
                    break;
                case 'NotFound':
                    showAlert('warning', 'There is not record with that information')
                    modalDelete.modal('hide')
                    break;
                default:
                    break;
            }
        },
        fail: function () {
            cleanMessages()
            console.log('Error')
            showAlert('danger', 'An error has ocurred, please contact with the systems administrator')
        }
    })
}