var ProjectoModel = function () {
    var self = this;

    self.init = function () {
        self.loadClient();
    }

    self.mnemonico = ko.observable();
    self.nombre = ko.observable();
    self.inicioEstimado = ko.observable();
    self.finEstimado = ko.observable();
    self.horasEstimadas = ko.observable();

    self.integrantes = ko.observableArray();
    self.integrante = ko.observable();
    self.integrantesProyecto = ko.observableArray();

    self.empresas = ko.observableArray();
    self.empresaSeleccionada = ko.observable();

    self.contactos = ko.observableArray();
    self.contactoSeleccionado = ko.observable();

    var Proyecto = {
        ContactoId: self.contactoSeleccionado,
        Mnemonico: self.mnemonico,
        EmpresaId: self.empresaSeleccionada,
        Integrantes: self.integrantesProyecto
    };

    self.reset = function () {
        self.contactoId = ko.observable();
        self.mnemonico = ko.observable();
        self.nombre = ko.observable();
        self.inicioEstimado = ko.observable();
        self.finEstimado = ko.observable();
        self.horasEstimadas = ko.observable();
        self.integrantesProyecto = ko.observableArray();
    }

    self.save = function (parametros) {
        alert('Se Grabó' + ko.toJSON(Proyecto));
    }

    self.empresaChange = function (parametros) {
        var parametros = ko.toJSON({ empresaId: Proyecto.EmpresaId() })

        self.loadContact(parametros);
    }

    self.loadClient = function () {
        $.ajax({
            type: "GET",
            url: '/Proyecto/ListarClientes',
            success: function (data) {
                self.empresas(data)
            },
            dataType: "json"
        });
    }

    self.loadContact = function (parametros) {
        $.ajax({
            type: "POST",
            url: '/Proyecto/ListarContacto',
            data: parametros,
            success: function (data) {
                self.contactos(data)
            },
            dataType: "json",
            contentType: "application/json"
        });
    }

    ko.bindingHandlers.autoComplete = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            var postUrl = allBindingsAccessor().source;
            var selectedObservableArrayInViewModel = valueAccessor();

            $(element).autocomplete({
                minLength: 2,
                autoFocus: true,
                source: function (request, response) {
                    $.ajax({
                        url: postUrl,
                        data: { patron: request.term },
                        dataType: "json",
                        type: "POST",
                        success: function (data) {
                            response(data);
                        }
                    });
                },
                select: function (event, ui) {
                    var selectedItem = ui.item;

                    self.integrantesProyecto.push({
                        IntegranteId: selectedItem.IntegranteId,
                        Nombre: selectedItem.Nombre,
                        EsEncargado: false
                    });

                }
            });
        }
    };

    self.init();
};

var viewModel = new ProjectoModel();
ko.applyBindings(viewModel);