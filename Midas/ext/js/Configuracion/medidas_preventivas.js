function MostrarMensaje(mensaje, tipo) {
    Swal.fire({
        position: 'center',
        icon: tipo,
        title: mensaje,
        showConfirmButton: false,
        timer: 2000
    })
}


