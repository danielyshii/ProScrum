var ProjectoModel = function () {
    var self = this;
    self.proyectos = ko.observableArray();

    self.refresh = function (parametros) {
        $.ajax({
            type: "POST",
            url: '/Proyecto/ListarProyectos',
            data: parametros,
            success: function (data) {
                self.proyectos(data)
            },
            dataType: "json",
            contentType: "application/json"
        });

    }

    self.updateProject = function (proyecto) {
        window.location = "/proyecto/edit/" + proyecto.ProyectoId;
    };

    self.removeProject = function (proyecto) {
        $.ajax({
            type: "POST",
            url: '/Proyecto/Delete',
            data: ko.toJSON({ id: proyecto.ProyectoId }),
            success: function (data) {
                self.refresh();
            },
            dataType: "json",
            contentType: "application/json"
        });
    };

};

var listaProyectoViewModel = new ProjectoModel();

var FiltroModel = function () {
    var self = this;
    self.empresaSeleccionada = ko.observable();
    self.empresas = ko.observableArray();
    self.descripcion = ko.observable();

    self.searchProject = function () {
        var parametros = ko.toJSON({ parametro: { empresaId: this.empresaSeleccionada(), descripcion: this.descripcion() } });
        listaProyectoViewModel.refresh(parametros)
    }

    self.refresh = function () {
        $.getJSON('/Proyecto/ListarClienteConProyectos', function (data) {
            self.empresas(data);
        });
    }
};

var filtroViewModel = new FiltroModel();


ko.applyBindings(listaProyectoViewModel, document.getElementById("pnl-ProyectosActivos"));
listaProyectoViewModel.refresh();

ko.applyBindings(filtroViewModel, document.getElementById("pnl-FiltrosConsultaProyecto"));
filtroViewModel.refresh();