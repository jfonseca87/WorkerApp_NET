// Globals
let btnSave

$(document).ready(function () {
    setDOMObjects()

    btnSave.click(function () {
        saveUser()
    })
})

function setDOMObjects() {
    btnSave = $('#btnSave')
}

function getUserFormData() {
    const formObject = {}
    const inputs = $('input')

    for (let input of inputs) {
        formObject[input.name] = input.value
    }

    return formObject
}

function saveUser() {
    const form = getUserFormData()

    $.ajax({
        method: 'POST',
        url: '/User/Register',
        data: form,
        success: function (data) {
            if (data.Message === 'Error') {
                let errors = createErrorMessageValidation(data.Errors)
                showAlert('danger', errors)
                return
            }

            clearLocalStorage()
            setDataUserId(data.User.UserId)
            setDataUserEmail(data.User.Email)

            const url = `${window.location.origin}/Person/Person`
            window.location.replace(url)
        },
        error: function () {
            console.log('Error')
            showAlert('danger', 'An error has ocurred, please contact with the system administrator')
        }
    })
}