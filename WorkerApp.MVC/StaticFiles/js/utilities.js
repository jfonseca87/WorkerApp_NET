function showAlert(type, message) {
    switch (type) {
        case 'success':
            const successAlert = createAlert('success', message)
            $('.message-container').html(successAlert)
            break
        case 'warning':
            const warningAlert = createAlert('warning', message)
            $('.message-container').html(warningAlert)
            break
        case 'danger':
            const dangerAlert = createAlert('danger', message)
            $('.message-container').html(dangerAlert)
            break
        case 'danger-modal':
            const dangerModalAlert = createAlert('danger', message)
            $('.danger-container-modal').html(dangerModalAlert)
            break
        default:
            break
    }
}

function createErrorMessageValidation(errors) {
    const message = '<p>There are some errors, please verify:</p>'
    let ulErrors = $('<ul></ul>')
    const htmlErrors = errors.map(error => {
        return `<li>${error}</li>`
    })

    ulErrors.html(htmlErrors)

    return `${message}${ulErrors.html()}`
}

function createAlert(type, message) {
    const alert = `<div class="alert alert-${type} alert-dismissible fade show" role="alert">
                        <div>${message}</div>
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                   </div>`

    return alert
}

function cleanModalMessage() {
    $('.danger-container-modal').html('')
}

function getParameters(parametersString) {
    if (parametersString === '') {
        return null
    } 

    const paramObj = {}

    parametersString = parametersString.substring(1)
    const parametersArray = parametersString.split('&')

    for (let parameter of parametersArray) {
        const parameterArray = parameter.split('=')
        paramObj[parameterArray[0]] = parameterArray[1]
    }

    return paramObj
}

function setDataUserId(userId) {
    window.localStorage.setItem('userId', userId)
}

function setDataUserEmail(email) {
    window.localStorage.setItem('userEmail', email)
}

function clearLocalStorage() {
    window.localStorage.clear()
}