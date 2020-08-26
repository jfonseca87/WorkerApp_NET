function redirecToWorker(id) {
    const url = `${window.location.origin}/Person/Person?id=${id}&read=true`
    window.location.href = url
}

