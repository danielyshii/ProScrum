var ProyectosEnProgreso = function () {

    var self = this;
    self.proyectos = ko.observableArray();

    self.init = function () {
        $.ajax({
            type: "POST",
            url: '/Proyecto/ProyectosEnProgreso',
            success: function (data) {
                self.proyectos(data)
            },
            dataType: "json"
        });
    }

    self.board = function (proyecto) {
        window.location = "/TaskBoard/Board/" + proyecto.ProyectoId;
    }

    self.init();
};

var listaProyectoViewModel = new ProyectosEnProgreso();
ko.applyBindings(listaProyectoViewModel);