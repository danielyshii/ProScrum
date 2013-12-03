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
        Nombre: self.nombre,
        EmpresaId: self.empresaSeleccionada,
        Integrantes: self.integrantesProyecto,
        InicioEstimado: self.inicioEstimado,
        FinEstimado: self.finEstimado,
        HorasEstimadas: self.horasEstimadas
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
        //alert('Se Grabó' + ko.toJSON(Proyecto));
        $.ajax({
            type: "POST",
            url: '/Proyecto/Create',
            data: ko.toJSON(Proyecto),
            dataType: "json",
            contentType: "application/json",
            success: function (data) {
                //self.contactos(data)
            }
            
        });
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
                minLength: 1,
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

    //Datepicker Knockout
    
    self.setToCurrentDate = function () {
        this.inicioEstimado(new Date());
        this.finEstimado(new Date());
    }
    
    ko.bindingHandlers.datepicker = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            //initialize datepicker with some optional options
            var options = allBindingsAccessor().datepickerOptions || {},
                $el = $(element);

            $el.datepicker(options);

            //handle the field changing
            ko.utils.registerEventHandler(element, "change", function () {
                var observable = valueAccessor();
                observable($el.datepicker("getDate"));
            });

            //handle disposal (if KO removes by the template binding)
            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $el.datepicker("destroy");
            });

        },
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor()),
                $el = $(element);

            //handle date data coming via json from Microsoft
            if (String(value).indexOf('/Date(') == 0) {
                value = new Date(parseInt(value.replace(/\/Date\((.*?)\)\//gi, "$1")));
            }

            var current = $el.datepicker("getDate");

            if (value - current !== 0) {
                $el.datepicker("setDate", value);
            }
        }
    };

    

    self.init();
};

var viewModel = new ProjectoModel();
ko.applyBindings(viewModel);