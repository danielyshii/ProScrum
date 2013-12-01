
var ProjectoModel = function (proyectos) {
    var self = this;
    self.proyectos = ko.observableArray(proyectos);

    self.updateProject = function (proyecto) {
        alert(proyecto.ProyectoId)
    };

    self.removeProject = function (proyecto) {
        alert(proyecto.ProyectoId);
    };

};

var proyectos = [];
$.getJSON('/Proyecto/ListProyects', function (data) {

    var viewModel = new ProjectoModel(data);
    ko.applyBindings(viewModel);

});



console.log("Loader cargado");