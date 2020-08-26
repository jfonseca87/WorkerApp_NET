// GLobal
let btnLogIn

$(document).ready(function () {
    setDOMObjects()

    btnLogIn.click(function () {
        logIn()
    })
})

function setDOMObjects() {
    btnLogIn = $('#btnLogin')
}

function setFormData() {
    const objForm = {}
    const inputs = $('input')

    for (let input of inputs) {
        objForm[input.name] = input.value
    }

    return objForm
}

function logIn() {
    const form = setFormData()

    $.ajax({
        method: 'POST',
        url: '/User/Login',
        data: form,
        success: function (data) {
            switch (data.Message) {
                case "Error":
                    let errors = createErrorMessageValidation(data.Errors)
                    showAlert('danger', errors)
                    break
                case "UserError":
                    showAlert('danger', 'There is a problem with the user, please verify email and password')
                    break
                case "Authenticated":
                    clearLocalStorage()
                    if (data.Role) {
                        location.replace(`${location.origin}/Home/Main`)
                        break
                    } 

                    if (data.PersonId !== null) {
                        location.replace(`${location.origin}/Person/Person?id=${data.PersonId}`)
                    } else {
                        setDataUserId(data.User.UserId)
                        setDataUserEmail(data.User.Email)
                        location.replace(`${location.origin}/Person/Person`)
                    }
                    break
                default:
                    break
            }
        },
        error: function () {
            console.log('Error')
            showAlert('danger', 'An error has ocurred, please contact with the system administrator')
        }
    })
}