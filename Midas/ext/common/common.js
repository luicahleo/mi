function MostrarMensaje(mensaje, tipo, tiempoDuracion) {
    Swal.fire({
        position: 'center',
        icon: tipo,
        title: mensaje,
        showConfirmButton: false,
        timer: tiempoDuracion
    })
}


