//ko.bindingHandlers.slider = {
//    init: function (element, valueAccessor, allBindingsAccessor) {
//        var options = allBindingsAccessor().sliderOptions || {};
//        $(element).slider(options);
//        ko.utils.registerEventHandler(element, "slidechange", function (event, ui) {
//            var observable = valueAccessor();
//            observable(ui.value);
//        });
//        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
//            $(element).slider("destroy");
//        });
//        ko.utils.registerEventHandler(element, "slide", function (event, ui) {
//            var observable = valueAccessor();
//            observable(ui.value);
//        });
//    },
//    update: function (element, valueAccessor) {
//        var value = ko.utils.unwrapObservable(valueAccessor());
//        if (isNaN(value)) value = 0;
//        $(element).slider("value", value);

//    }
//};


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

var ConfigurarModel = function () {

    var self = this;
    self.ProyectoId = ko.observable();
    self.EsConfiguracion = ko.observable(true);
    self.EsDefinicion = ko.observable(false);
    self.EsResumen = ko.observable(false);
    self.SemanasEstimadas = ko.observable(2);

    //public bool EsConfiguracionSprint { get; set; }
    //public bool EsDefinicionProductBacklog { get; set; }
    //public bool EsResumenConfiguracion { get; set; }

    self.init = function () {
        
    }

    self.init();
};

var listaProyectoViewModel = new ConfigurarModel();
ko.applyBindings(listaProyectoViewModel);