
var ProjectoModel = function () {
    var self = this;
    self.proyectos = ko.observableArray([]);

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
        alert(proyecto.ProyectoId)
    };

    self.removeProject = function (proyecto) {
        alert(proyecto.ProyectoId);
    };

};

var listaProyectoViewModel = new ProjectoModel();

var FiltroModel = function (empresas) {
    var self = this;
    self.empresaSeleccionada = ko.observable();
    self.empresas = ko.observableArray(empresas);
    self.descripcion = ko.observable();

    self.searchProject = function () {
        var parametros = ko.toJSON({ parametro: { empresaId: this.empresaSeleccionada(), descripcion: this.descripcion() } });
        debugger;
        listaProyectoViewModel.refresh(parametros)
    }
};




ko.applyBindings(listaProyectoViewModel, document.getElementById("pnl-ProyectosActivos"));
listaProyectoViewModel.refresh();

$.getJSON('/Proyecto/ListarCliente', function (data) {
    var viewModel = new FiltroModel(data);
    ko.applyBindings(viewModel, document.getElementById("pnl-FiltrosConsultaProyecto"));

});