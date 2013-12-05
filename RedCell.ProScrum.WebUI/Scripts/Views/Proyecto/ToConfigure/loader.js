var PorConfigurarModel = function () {

    var self = this;
    self.proyectos = ko.observableArray();

    self.init = function ()
    {
        $.ajax({
            type: "POST",
            url: '/Proyecto/ProyectosPorConfigurar',
            success: function (data) {
                self.proyectos(data)
            },
            dataType: "json"
        });
    }

    self.configure = function (proyecto) {
        window.location = "/Proyecto/Configure/"+proyecto.ProyectoId;
    }

    self.init();
};

var listaProyectoViewModel = new PorConfigurarModel();
ko.applyBindings(listaProyectoViewModel);