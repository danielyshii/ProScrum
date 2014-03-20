function BoardWindowManager() {

    var base = this;

    base.config = {
        BoardUserStoryColorUpdate: null,
        BoardUserStoryActivityChange: null,
        BoarUserStoryStateChange: null
    };

    base.hide = function () {
        $('body').removeClass('window-up');
        $('div.window-overlay-userstory div.window div.window-wrapper div.js-dialog-container').contents().remove('div');
    }

    base.show = function (data) {
        $('body').addClass('window-up');
        $("div.window-overlay-userstory div.window div.window-wrapper div.js-dialog-container").html(data);
        base.FuncionesOVerlayUserStory.inicializarHandler();
    }


    base.AjaxCalls = {

        changeUserStoryColor: function (userStoryId, color, isSelected) {

            if (isSelected == true) {
                color = null;
            }

            $.ajax({
                type: "POST",
                url: '/TaskBoard/ChangeUserStoryColor',
                data: JSON.stringify({ 'usid': userStoryId, 'color': color }),
                dataType: "json",
                contentType: "application/json",
                success: base.FuncionesOVerlayUserStory.animateChangeColor
            });

        },

        addActivity: function (description, userStoryId) {

            $.ajax({
                type: "POST",
                url: '/TaskBoard/AddActivity',
                data: JSON.stringify({ 'descripcion': description, 'uid': userStoryId }),
                dataType: "json",
                contentType: "application/json",
                success: base.FuncionesOVerlayUserStory.animateAddActivity
            });

        },

        deleteActivity: function (domElement, aId) {
            $.ajax({
                type: "POST",
                url: '/TaskBoard/DeleteActivity',
                data: JSON.stringify({ 'aid': aId }),
                dataType: "json",
                contentType: "application/json",
                success: function (response) {

                    base.FuncionesOVerlayUserStory.animateDeleteActivity(domElement, response)
                }
            });
        },

        endActivity: function (domElement, aId) {

            $.ajax({
                type: "POST",
                url: '/TaskBoard/EndActivity',
                data: JSON.stringify({ 'aid': aId }),
                dataType: "json",
                contentType: "application/json",
                success: function (response) { base.FuncionesOVerlayUserStory.animateEndActivity(domElement, response) }
            });

        },

        assignUser: function (userStoryId) {

            $.ajax({
                type: "POST",
                url: '/TaskBoard/AssignUserStory',
                data: JSON.stringify({ 'usid': userStoryId }),
                dataType: "json",
                contentType: "application/json",
                success: base.FuncionesOVerlayUserStory.animateAssignUser
            });
        }
    },

    base.FuncionesOVerlayUserStory = {

        inicializarHandler: function () {
            base.FuncionesOVerlayUserStory.loadSelectedColor();
            base.FuncionesOVerlayUserStory.addEventsToUserStory();

        },

        addEventsToUserStory: function () {
            // Asignar Usuario
            $('a.js-change-card-members').on('click', function () {

                var userStoryId = $('#hddUserStoryId').val();

                //alert('Click en Asignar');

                base.AjaxCalls.assignUser(userStoryId);
            })

            // Agregar Actividad
            $('div.js-add-activity').on('click', function () {

                var descripcion = $("textarea.js-new-comment-input").val();
                var userStoryId = $('#hddUserStoryId').val();

                if (descripcion != '') {
                    base.AjaxCalls.addActivity(descripcion, userStoryId);
                }
                else {
                    base.FuncionesOVerlayUserStory.animateNewActivityError();
                }

            });

            // Eliminar Actividad
            $('div.js-activity-list').on('click', 'div.phenom  a.js-delete-activity', function () {

                var actividadId = $(this).attr('actividadId');

                //alert('Click en Eliminar Actividad: ' + actividadId);

                base.AjaxCalls.deleteActivity(this, actividadId);

                //base.FuncionesOVerlayUserStory.animateDeleteActivity(this);
            });

            // Terminar Actividad
            $('div.js-activity-list').on('click', 'div.phenom div.creator div.check-div input.js-end-activity', function () {

                var actividadId = $(this).attr('actividadId');

                //alert('Click en Terminar Actividad: ' + actividadId);

                base.AjaxCalls.endActivity(this, actividadId);

                //base.FuncionesOVerlayUserStory.animateEndActivity(this);

            });

            // Cambiar Color
            $('span.colored-label').on('click', function () {

                var isSelected = $(this).hasClass('card-label-selected');
                var userStoryId = $('#hddUserStoryId').val();
                var newColorId = $(this).attr('color-id');

                //alert('Click en Nuevo Color: ' + newColorId + "  En US: " + userStoryId);

                //base.FuncionesOVerlayUserStory.animateChangeColor(newColorId, isSelected);

                base.AjaxCalls.changeUserStoryColor(userStoryId, newColorId, isSelected)

            });
        },

        animateDeleteActivity: function (domElement, response) {
            $(domElement).parent().remove();

            base.config.BoardUserStoryActivityChange(response);
        },

        animateEndActivity: function (domElement, response) {
            $(domElement).attr('disabled', 'disabled');
            $(domElement).parent().parent().parent().find('a.js-delete-activity').remove();

            base.config.BoardUserStoryActivityChange(response);
        },

        animateChangeColor: function (data) {

            base.config.BoardUserStoryColorUpdate(data);

            $('span.colored-label').each(function (index) {

                if ($(this).hasClass('card-label-selected')) {
                    $(this).removeClass('card-label-selected');
                    $(this).addClass('label-to-assign');
                }
                else {
                    if (index == data.NewColor) {
                        $(this).removeClass('label-to-assign');
                        $(this).addClass('card-label-selected');
                    }
                }

            });

        },

        animateAddActivity: function (activityData) {

            var nuevaActividad = "<div class='phenom clearfix'>" +
                                    "<div class='creator js-show-mem-menu'>" +
                                        "<div class='check-div'>" +
                                            "<input type='checkbox' actividadId ='" + activityData.ActividadId + "' class='js-end-activity' title='Seleccione si desea Terminar Actividad'>" +
                                        "</div>" +
                                    "</div> " +
                                    "<div class='phenom-desc'>" + activityData.Descripcion + "</div>" +
                                    "<a title='Eliminar Actividad.' actividadId ='" + activityData.ActividadId + "' class='deleter button-link js-delete-activity'><span class='icon-sm icon-trash'></span></a>" +
                                 "</div>";

            $("textarea.js-new-comment-input").val('');

            $('div.js-activity-list').prepend(nuevaActividad);

            base.config.BoardUserStoryActivityChange(activityData);

        },

        animateAssignUser: function (data) {
            $('a.js-change-card-members').remove();
            $('div.js-change-card-container').append(data.NombreUsuario);

            base.config.BoarUserStoryStateChange(data);

        },

        animateNewActivityError: function () {
            if (!$("textarea.js-new-comment-input").hasClass("shake")) {
                $("textarea.js-new-comment-input").addClass("shake");

                setTimeout(function () {
                    $("textarea.js-new-comment-input").removeClass("shake");
                }, 1000);
            }
        },

        loadSelectedColor: function () {
            var selectedLabelStyle = 'card-label-selected';
            var selectedColor = parseInt($('#hddIdColor').val());

            if (!isNaN(selectedColor)) {
                $('span.colored-label').each(function (index) {

                    if (index == selectedColor) {
                        $(this).removeClass('label-to-assign');
                        $(this).addClass('card-label-selected');

                        return false;
                    }

                });

            }

        },

        removeEventsFromUserStory: function () {
            $('a.js-change-card-members').off('click');
            $('div.js-add-activity').off('click');
            $('div.js-activity-list').off('click');
            $('span.label-to-assign').off('click');
        }

    }

    base.onClose = function () {

        $("div.window-overlay-userstory div.window div.window-wrapper div.dialog-close-button").on("click", "a.js-close-window", function () {
            base.FuncionesOVerlayUserStory.removeEventsFromUserStory();
            base.hide();
        });
    }

    base.init = function (config) {

        $.extend(base.config, config);
        base.onClose();

        console.log('I am Initializing!');
    };
}

function BoardValidateWindowManager() {

    var base = this;

    base.hide = function () {
        $('body').removeClass('window-up-validate');
        $('div.window-overlay-validate div.window div.window-wrapper div.js-dialog-container').contents().remove('div');
    }

    base.show = function (data) {
        $('body').addClass('window-up-validate');
        $("div.window-overlay-validate div.window div.window-wrapper div.js-dialog-container").html(data);

        base.Funciones.InicializarHandler();
    }

    base.onClose = function () {

        $("div.window-overlay-validate div.window div.window-wrapper div.dialog-close-button").on("click", "a.js-close-window", function () {
            base.Funciones.RemoveHandler();
            base.hide();
        });
    }

    base.AjaxCall = {

        SaveAcceptance: function (uid) {

            $.ajax({
                type: "POST",
                url: '/TaskBoard/SaveAcceptance',
                data: JSON.stringify({ 'UserStoryId': uid }),
                dataType: "json",
                contentType: "application/json",
                success: base.Eventos.SaveAcceptanceSuccess
            });

        },

    }

    base.Funciones = {

        InicializarHandler: function () {

            //Accept Button
            $('div.js-validate-user-story-div').on('click', 'input#validate-user-story-btn-ok', function () {
                base.Eventos.SaveAcceptanceButtonHandler();
            });

            //Close Button
            $('div.js-validate-user-story-div').on('click', 'input#validate-user-story-btn-noOk', function () {
                base.Eventos.ShowRejectionFormButtonHandler();
            });


        },

        RemoveHandler: function () {
            $('div.window-main-footer').off('click');
        }

    }

    base.Eventos = {

        ShowRejectionFormButtonHandler: function () {

            $('div.js-validate-user-story-div').addClass('hide');
            $('div.js-reject-user-story-div').removeClass('hide');

        },

        SaveAcceptanceButtonHandler: function () {
            
            alert('No Go');
            //var uid = $('input#validate-user-story-id').val();
            //base.AjaxCall.SaveAcceptance(uid);
        },

        SaveAcceptanceSuccess: function (data) {

            //Cerrar Ventana
            base.Funciones.RemoveHandler();
            base.hide();

            //Llamar Handler del Board de Mover Estado

        }

    }

    base.init = function (config) {
        $.extend(base.config, config);
        base.onClose();

        console.log('I am Initializing!');
    }
}

function BoardBlockWindowManager() {
    var base = this;

    base.config = {
        BoardUserStoryBlockUpdate: null
    }

    base.hide = function () {
        $('body').removeClass('window-up-block');
        $('div.window-overlay-block div.window div.window-wrapper div.js-dialog-container').contents().remove('div');
    }

    base.show = function (data) {
        $('body').addClass('window-up-block');
        $("div.window-overlay-block div.window div.window-wrapper div.js-dialog-container").html(data);

        base.Funciones.InicializarHandler();
    }

    base.onClose = function () {

        $("div.window-overlay-block div.window div.window-wrapper div.dialog-close-button").on("click", "a.js-close-window", function () {
            base.Funciones.RemoveHandler();
            base.hide();
        });
    }

    base.AjaxCall = {

        saveBlock: function (uid, tbid, desc) {

            $.ajax({
                type: "POST",
                url: '/TaskBoard/SaveBlock',
                data: JSON.stringify({ 'UserStoryId': uid, 'TipoBloqueoId': tbid, 'Descripcion': desc }),
                dataType: "json",
                contentType: "application/json",
                success: base.Eventos.AnimateBlockUS
            });

        },

    }

    base.Funciones = {

        InicializarHandler: function () {

            //Accept Button
            $('div.window-main-footer').on('click', 'input#block-user-story-btn-ok', function () {
                base.Eventos.AddSaveButtonHandler();
            });

            //Close Button
            $('div.window-main-footer').on('click', 'input#block-user-story-btn-cancel', function () {
                base.Eventos.AddCloseButtonHandler();
            });


        },

        RemoveHandler: function () {
            $('div.window-main-footer').off('click');
        }


    }

    base.Eventos = {

        AddCloseButtonHandler: function () {
            base.Funciones.RemoveHandler();
            base.hide();
        },

        AddSaveButtonHandler: function () {

            var uid = null,
                tbid = null,
                desc = null,
                isValid = true;
            //Obtener Valores de Envio de formulario
            uid = $('#block-user-story-id').val();
            tbid = $('select[name="TipoBloqueoId"]').val();
            desc = $('textarea[name="Descripcion"]').val();

            if (isNaN(tbid))
            {
                base.Eventos.AnimateInputError($('select[name="TipoBloqueoId"]'));
                isValid = false;
            }

            if (!(desc && desc != null && desc != ''))
            {
                base.Eventos.AnimateInputError($('textarea[name="Descripcion"]'));
                isValid = false;
            }

            //alert(uid);
            //alert(tbid);
            //alert(desc);

            //Llamar a SaveBlock
            if (isValid)
            {
                base.AjaxCall.saveBlock(uid, tbid, desc);
            }
        },

        AnimateBlockUS: function (response) {

            //Llamar a Cerrar Ventana
            base.Funciones.RemoveHandler();
            base.hide();

            //Llamar a evento delegado de asignar badge de block
            base.config.BoardUserStoryBlockUpdate(response);
        },

        AnimateInputError: function (jqDomElement) {
            if (!jqDomElement.hasClass("shake")) {
                jqDomElement.addClass("shake");

                setTimeout(function () {
                    jqDomElement.removeClass("shake");
                }, 1000);
            }
        },

    }

    base.init = function (config) {
        $.extend(base.config, config);
        base.onClose();

        console.log('I am Initializing!');
    }
}