ko.bindingHandlers.slider = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        //initialize the control
        var options = allBindingsAccessor().sliderOptions || {};
        $(element).slider(options);

        //handle the value changing in the UI
        ko.utils.registerEventHandler(element, "slidechange", function () {
            //would need to do some more work here, if you want to bind against non-observables
            var observable = valueAccessor();
            observable($(element).slider("value"));
        });

    },
    //handle the model value changing
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        $(element).slider("value", value);

    }
};

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

var ConfigurarModel = function () {

    var self = this;
    self.ProyectoId = ko.observable();
    self.EsConfiguracion = ko.observable(true);
    self.EsDefinicion = ko.observable(false);
    self.EsResumen = ko.observable(false);
    self.SemanasEstimadas = ko.observable(2);
    self.UsarSecuencial = ko.observable(true);
    self.NombreSprintInicial = ko.observable("Sprint 1");
    self.FechaInicioSprintInicial = ko.observable();
    self.ObjetivoSprint = ko.observable("");

    self.DescripcionUserStory = ko.observable("");
    self.HorasEstimadas = ko.observable(null);
    self.PerteneceSprintInicial = ko.observable(false);

    self.BacklogUserStories = ko.observableArray([]);
    self.SprintUserStories = ko.observableArray([]);

    self.init = function () {
        self.ProyectoId($("#hd_ProyectoId").val());
    }

    self.toDefinition = function () {

        if (!validarConfiguracion())
            return false;

        self.EsConfiguracion(false);
        self.EsDefinicion(true);
        self.EsResumen(false);
    }

    self.toResume = function () {        

        self.EsConfiguracion(false);
        self.EsDefinicion(false);
        self.EsResumen(true);
    }

    self.backToConfiguration = function () {
        self.EsConfiguracion(true);
        self.EsDefinicion(false);
        self.EsResumen(false);
    }

    self.backToDefinition = function () {
        self.EsConfiguracion(false);
        self.EsDefinicion(true);
        self.EsResumen(false);
    }

    self.addUserStory = function () {

        if (!validarUserStory())
            return false;

        var userStory = { DescripcionUserStory: self.DescripcionUserStory(), HorasEstimadas: self.HorasEstimadas(), PerteneceSprintInicial: self.PerteneceSprintInicial() };
        if (self.PerteneceSprintInicial() == true)
            self.SprintUserStories.push(userStory);
        else
            self.BacklogUserStories.push(userStory);

        self.cleanUserStory();
    }

    self.cleanUserStory = function () {
        self.DescripcionUserStory("");
        self.HorasEstimadas(null);
        self.PerteneceSprintInicial(false);
    }

    self.showBacklogUserStories = function () {
        alert(ko.toJSON(self.BacklogUserStories));
    }

    self.showInitialSprintUserStories = function () {
        alert(ko.toJSON(self.SprintUserStories));
    }

    self.showAddActivity = function () {

    }

    self.showInitialSprintStart = function () {

        var result = self.FechaInicioSprintInicial().getDate() + '/' + (self.FechaInicioSprintInicial().getMonth()+ 1) + '/' + self.FechaInicioSprintInicial().getFullYear();

        return result;
    }

    self.showInitialSprintEnd = function () {
        
        var endDate = new Date();

        endDate.setDate((self.FechaInicioSprintInicial().getDate() + 14));

        var result = endDate.getDate() + '/' + (endDate.getMonth() + 1) + '/' + endDate.getFullYear();

        return result;

    }

    self.init();
};

var listaProyectoViewModel = new ConfigurarModel();
ko.applyBindings(listaProyectoViewModel);

//VALIDACIONES

function validarConfiguracion() {

    var semanasEstimadas = document.getElementById("txt-semanasEstimadas");
    var usarSecuencial = document.getElementById("chb-usarSecuencial");
    var nombreSprintInicial = document.getElementById("txt-nombreSprintInicial");
    var fechaInicio = document.getElementById("dtp-inicio");
    var objetivoSprint = document.getElementById("txt-objetivoSprint");

    var mensaje = "";

    if (semanasEstimadas.value == "")
        mensaje += "-Semanas Estimadas\n";

    if (usarSecuencial.checked == true && nombreSprintInicial.value == "")
        mensaje += "-Nombre del Sprint Inicial\n";

    if (fechaInicio.value == "")
        mensaje += "-Fecha de Inicio del Sprint Actual\n";

    if (objetivoSprint.value == "")
        mensaje += "-Objetivo del Sprint\n";
    
    if (mensaje.length > 0) {
        mensaje = "Por favor, ingrese información válida en:\n\n" + mensaje;
        alert(mensaje);
        return false;
    }

    return true;
}

function validarUserStory() {

    var descripcionUserStory = document.getElementById("txt-descripcionUserStory");
    var horasEstimadas = document.getElementById("txt-horasEstimadas");

    var mensaje = "";

    if (descripcionUserStory.value == "")
        mensaje += "-Descripción\n";

    if (horasEstimadas.value == "")
        mensaje += "-Horas Estimadas\n";

    if (mensaje.length > 0) {
        mensaje = "Por favor, ingrese información válida en:\n\n" + mensaje;
        alert(mensaje);
        return false;
    }

    return true;
}

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}