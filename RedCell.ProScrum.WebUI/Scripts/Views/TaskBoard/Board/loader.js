﻿
function BoardController() {

    var base = this;

    base.Data = {
        ToDoState: 1,
        InProcessState: 2,
        ToVerifyState: 3,
        DoneState: 4
    }

    base.Controles = {
        WindowManager: new BoardWindowManager(),
        ValidateWindowManager: new BoardValidateWindowManager(),
        BlockWindowManager: new BoardBlockWindowManager()
    }

    base.AjaxCall = {

        EndUserStoryInProcess: function (uid) {

            $.ajax({
                type: "POST",
                url: '/TaskBoard/EndUserStoryInProcess',
                data: JSON.stringify({ 'UserStoryId': uid }),
                dataType: "json",
                contentType: "application/json",
                success: base.EventosBoardRender.OnUserStoryStateChange
            });

        },

    }

    base.Funciones = {

        LoadBoardData: function () {

            console.log('Yaaaay llamará :D');

            var proyectoId = $('#hddProyectoId').val();

            $.ajax({
                type: "POST",
                url: '/TaskBoard/ListBoardUserStories',
                data: JSON.stringify({ 'id': proyectoId }),
                dataType: "json",
                contentType: "application/json",
                success: base.Eventos.OnBoardDataLoad
            });

        },

        BuildColumns: function (data) {

            var jqColumnContainer = $('div.js-userstory-list');

            var userStoriesArray = new Array();

            $.each(data.BoardColumns, function (index, columnData) {

                var columnString = '<div class="list">' +
                                        '<div class="list-header non-empty clearfix editable">' +
                                            '<h2 class="list-header-name hide-on-edit current">' + columnData.Descripcion + '</h2>' +
                                        '</div>' +
                                        '<div class="list-cards fancy-scrollbar js-column-container clearfix" column-id-attr="' + columnData.BoardColumnId + '" style="min-height: 40px;"></div>' +
                                    '</div>';

                jqColumnContainer.append(columnString);

            })

            base.Funciones.BuildUserStories(data.UserStories);

        },

        BuildUserStories: function (userStoriesData) {

            var jqColumnColection = $('div[column-id-attr]');

            $.each(userStoriesData, function (index, userStoryData) {

                var userStoryContainer = jqColumnColection.filter('[column-id-attr=' + userStoryData.EstadoUserStoryId + ']');

                var lockBadge = '',
                    activityBadge = '',
                    verifyIcon = '',
                    inProcessIcon = '',
                    colorString = '',
                    lockIcon = '';

                if (userStoryData.EstaBloqueada == true) {
                    lockBadge = '<div class="badge badge-state-image-only" title="Este User Story se encuentra Bloqueado">' +
                                    '<span class="badge-icon icon-sm icon-lock"></span>' +
                                '</div>';
                }
                else {
                    lockIcon = '<div class="member js-block-click" title="Seleccione si desea bloquear el User Story">' +
                                '<span class="member-initials">B</span>' +
                               '</div>';
                }

                if (userStoryData.NumeroActividadTotal > 0) {
                    activityBadge = '<div class="badge js-user-story-activity-badge" title="Actividades Terminadas / Actividades Totales">' +
                                        '<span class="badge-icon icon-sm icon-checklist"></span>' +
                                        '<span class="badge-text">' + userStoryData.NumeroActividadTerminada + '/' + userStoryData.NumeroActividadTotal + '</span>' +
                                    '</div>';
                }

                if (userStoryData.Color != null) {

                    for (var i = 0; i < 5; i++) {
                        if (i == userStoryData.Color) {
                            switch (userStoryData.Color) {
                                case 0:
                                    colorString = '<span class="card-label card-label-green"></span>';
                                    break;
                                case 1:
                                    colorString = '<span class="card-label card-label-yellow"></span>';
                                    break;
                                case 2:
                                    colorString = '<span class="card-label card-label-orange"></span>';
                                    break;
                                case 3:
                                    colorString = '<span class="card-label card-label-red"></span>';
                                    break;
                                case 4:
                                    colorString = '<span class="card-label card-label-blue"></span>';
                                    break;
                                default:
                                    break;

                            }
                        }
                    }
                }

                if (!userStoryData.EstaBloqueada) {
                    if (userStoryData.EstadoUserStoryId == base.Data.ToVerifyState && userStoryData.NumeroActividadTotal > 0 && userStoryData.NumeroActividadTerminada == userStoryData.NumeroActividadTotal) {
                        verifyIcon = '<div class="member js-validate-click" title="Seleccione si desea iniciar la validación del User Story">' +
                                            '<span class="member-initials">👍</span>' +
                                        '</div>';
                    }

                    if (userStoryData.EstadoUserStoryId == base.Data.InProcessState && userStoryData.NumeroActividadTotal > 0 && userStoryData.NumeroActividadTerminada == userStoryData.NumeroActividadTotal) {
                        inProcessIcon = '<div class="member js-in-process-click" title="Seleccione si desea notificar el fin del desarrollo del User Story">' +
                                            '<span class="member-initials">✓</span>' +
                                        '</div>';
                    }
                }

                var userStoryBadges = '<div class="badges">' + lockBadge + activityBadge + '</div>';

                var userStoryString = '<div class="list-card js-user-story-container" board-user-story-id = ' + userStoryData.UserStoryId + '>' +
                                            '<div class="list-card-labels clearfix">' +
                                               colorString +
                                            '</div>' +
                                            '<div class="list-card-details clearfix" user-story-id="' + userStoryData.UserStoryId + '">' +
                                                '<a class="list-card-title clear js-card-name" title="' + userStoryData.Descripcion + '">' +
                                                    '<span class="card-short-id hide"></span>' + userStoryData.Codigo + '</a>' +
                                                userStoryBadges +
                                                '<div class="list-card-members">' +
                                                    verifyIcon +
                                                    inProcessIcon +
                                                    lockIcon +
                                                '</div>' +
                                            '</div>' +
                                        '</div>'

                userStoryContainer.append(userStoryString);

            });


            base.Eventos.OnClickCard();
            base.Eventos.OnClickValidate();
            base.Eventos.OnClickToProcess();
            base.Eventos.OnClickBlock();
        },

        LoadUserStoryWindow: function (userStoryId) {
            $.get('/TaskBoard/UserStoryDetail/' + userStoryId, base.Eventos.OnUserStoryDetailSuccess);
        },

        LoadBlockUserStoryWindow: function (userStoryId) {
            $.get('/TaskBoard/BlockUserStory/' + userStoryId, base.Eventos.OnUserStoryBlockSuccess);
        },

        LoadValidateUserStoryWindow: function (userStoryId) {
            $.get('/TaskBoard/ValidateUserStory/' + userStoryId, base.Eventos.OnUserStoryValidateSuccess);
        },

        // { UserStoryId = 4, NumeroActividadTerminada = 4, NumeroActividadTotal = 5 }
        UpdateListActivity: function (response) {


                var jqUSContainer = $('div.js-user-story-container').filter('[board-user-story-id=' + response.UserStoryId + ']');

                var toBeAppended = '<span class="badge-icon icon-sm icon-checklist"></span>' +
                                   '<span class="badge-text">' + response.NumeroActividadTerminada + '/' + response.NumeroActividadTotal + '</span>';

                if (!response.NumeroActividadTotal || response.NumeroActividadTotal == 0) {
                    toBeAppended = ''
                }

                var jqActivityBadgeContainer = jqUSContainer.find('div.list-card-details div.badges div.js-user-story-activity-badge');

                if (jqActivityBadgeContainer.length > 0) {
                    jqActivityBadgeContainer.empty();
                }
                else {
                    jqActivityBadgeContainer = jqUSContainer.find('div.list-card-details div.badges');

                    var activityBadge = '<div class="badge js-user-story-activity-badge" title="Actividades Terminadas / Actividades Totales"></div>';
                    jqActivityBadgeContainer.append(activityBadge);
                    jqActivityBadgeContainer = jqUSContainer.find('div.list-card-details div.badges div.js-user-story-activity-badge');
                }

                jqActivityBadgeContainer.append(toBeAppended);



        }

    }

    base.Eventos = {

        OnBoardDataLoad: function (data) {

            base.Funciones.BuildColumns(data);

        },

        OnClickCard: function () {
            $("div.list-cards").on("click", "div.list-card div.list-card-details", function (e) {
                e.stopPropagation();
                var userStoryId = $(this).attr('user-story-id');

                base.Funciones.LoadUserStoryWindow(userStoryId);
            });
        },

        OnClickValidate: function () {

            $("div.list-cards").on("click", "div.list-card div.list-card-details div.list-card-members div.js-validate-click", function (e) {
                e.stopPropagation();

                var userStoryId = $(this).parent().parent().attr('user-story-id');

                base.Funciones.LoadValidateUserStoryWindow(userStoryId);

            });

        },

        OnClickToProcess: function () {
            $("div.list-cards").on("click", "div.list-card div.list-card-details div.list-card-members div.js-in-process-click", function (e) {
                e.stopPropagation();

                var userStoryId = $(this).parent().parent().attr('user-story-id');

                base.AjaxCall.EndUserStoryInProcess(userStoryId);

            });
        },

        OnClickBlock: function () {
            $("div.list-cards").on("click", "div.list-card div.list-card-details div.list-card-members div.js-block-click", function (e) {
                e.stopPropagation();

                var userStoryId = $(this).parent().parent().attr('user-story-id');

                base.Funciones.LoadBlockUserStoryWindow(userStoryId);
            });
        },

        OnUserStoryDetailSuccess: function (data) {
            base.Controles.WindowManager.show(data);
        },

        OnUserStoryBlockSuccess: function (data) {
            base.Controles.BlockWindowManager.show(data);
        },

        OnUserStoryValidateSuccess: function (data) {
            base.Controles.ValidateWindowManager.show(data);
        }
    }

    base.EventosBoardRender = {

        // { UserStoryId = 4, NewColor = null }
        OnUserStoryColorUpdate: function (response) {

            var jqUSContainer = $('div.js-user-story-container').filter('[board-user-story-id=' + response.UserStoryId + ']');

            var colorString = '';

            switch (response.NewColor) {
                case 0:
                    colorString = '<span class="card-label card-label-green"></span>';
                    break;
                case 1:
                    colorString = '<span class="card-label card-label-yellow"></span>';
                    break;
                case 2:
                    colorString = '<span class="card-label card-label-orange"></span>';
                    break;
                case 3:
                    colorString = '<span class="card-label card-label-red"></span>';
                    break;
                case 4:
                    colorString = '<span class="card-label card-label-blue"></span>';
                    break;
                default:
                    colorString = ''
                    break;

            }

            jqUSContainer.find('div.list-card-labels').empty();
            jqUSContainer.find('div.list-card-labels').append(colorString);

        },

        // { UserStoryId = 4, NumeroActividadTerminada = 4, NumeroActividadTotal = 5, NewUserStoryState = 5 }
        OnUserStoryActivitiesChange: function (response) {


            base.Funciones.UpdateListActivity(response);

            var jqUSContainer = $('div.js-user-story-container').filter('[board-user-story-id=' + response.UserStoryId + ']');

            var jqMemberContainer = jqUSContainer.find('div.list-card-details div.list-card-members')

            if (response.IsUserStoryStateAfected == true) {

                var toAppend = '';

                if (response.NewUserStoryState == base.Data.ToVerifyState) {
                    toAppend = '<div class="member js-in-process-click" title="Seleccione si desea notificar el fin del desarrollo del User Story">' +
                                    '<span class="member-initials">✓</span>' +
                               '</div>';

                }
                else if (response.NewUserStoryState == base.Data.DoneState) {
                    toAppend = '<div class="member js-validate-click" title="Seleccione si desea iniciar la validación del User Story">' +
                                    '<span class="member-initials">👍</span>' +
                                '</div>';
                }

                jqMemberContainer.prepend(toAppend);
            }
            else {

                if (response.NewUserStoryState == base.Data.InProcessState) {
                    jqMemberContainer.find('div.js-in-process-click').remove();
                }
                else if (response.NewUserStoryState == base.Data.ToVerifyState) {
                    jqMemberContainer.find('div.js-validate-click').remove();
                }

            }

        },

        // { NuevoEstadoUserStory = 2, UserStoryId = 2 , NumeroActividadTerminada = 4, NumeroActividadTotal = 5}
        OnUserStoryStateChange: function (response) {

            var jqUSContainer = $('div.js-user-story-container').filter('[board-user-story-id=' + response.UserStoryId + ']');

            jqUSContainer.remove();

            var jqColumnContainer = $('div.js-column-container').filter('[column-id-attr=' + response.NuevoEstadoUserStory + ']');

            jqColumnContainer.append(jqUSContainer);

            //Retirar o Agregar nuevo estado de UserStory

            var jqMemberContainer = jqUSContainer.find('div.list-card-details div.list-card-members');

            if (response.NuevoEstadoUserStory == base.Data.ToVerifyState) {
                jqMemberContainer.find('div.js-in-process-click').remove();
            }
            else if (response.NuevoEstadoUserStory == base.Data.DoneState) {
                jqMemberContainer.find('div.js-validate-click').remove();
            }
            else if (response.NuevoEstadoUserStory == base.Data.InProcessState) {
                jqMemberContainer.find('div.js-validate-click').remove();
            }

            base.Funciones.UpdateListActivity(response);

        },

        // { IdUserStory = 11, IsBloqued = true }
        OnUserStoryBlockChange: function (response) {

            var jqUSContainer = $('div.js-user-story-container').filter('[board-user-story-id=' + response.UserStoryId + ']');

            var toBeAppended = '<div class="badge badge-state-image-only" title="Este User Story se encuentra Bloqueado">' +
                                    '<span class="badge-icon icon-sm icon-lock"></span>' +
                                '</div>';

            var jqBlockBadgeContainer = jqUSContainer.find('div.list-card-details div.badges');

            jqBlockBadgeContainer.prepend(toBeAppended);

            var jqBlockMemberContainer = jqUSContainer.find('div.list-card-details div.list-card-members div.js-block-click')

            jqBlockMemberContainer.remove();


            var jqMemberContainer = jqUSContainer.find('div.list-card-details div.list-card-members');

            jqMemberContainer.find('div.js-in-process-click').remove();
            jqMemberContainer.find('div.js-validate-click').remove();

        }

    }

    base.init = function () {
        console.log('Init BoardController');
        base.Controles.WindowManager.init({
            BoardUserStoryColorUpdate: base.EventosBoardRender.OnUserStoryColorUpdate,
            BoardUserStoryActivityChange: base.EventosBoardRender.OnUserStoryActivitiesChange,
            BoarUserStoryStateChange: base.EventosBoardRender.OnUserStoryStateChange
        });
        base.Controles.ValidateWindowManager.init({
            BoarUserStoryStateChange: base.EventosBoardRender.OnUserStoryStateChange
        });
        base.Controles.BlockWindowManager.init({
            BoardUserStoryBlockUpdate: base.EventosBoardRender.OnUserStoryBlockChange
        });
        base.Funciones.LoadBoardData();
    };

}

$(document).ready(function () {
    var controlador = new BoardController();
    controlador.init();
});