
function BoardController() {

    var base = this;

    base.Controles = {
        WindowManager: new BoardWindowManager(),
        ValidateWindowManager : new BoardValidateWindowManager()
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
                                        '<div class="list-cards fancy-scrollbar clearfix" column-id-attr="' + columnData.BoardColumnId + '" style="min-height: 40px;"></div>' +
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
                    colorString = '',
                    lockIcon = '';

                if (userStoryData.EstaBloqueada == true) {
                    lockBadge = '<div class="badge badge-state-image-only">' +
                                    '<span class="badge-icon icon-sm icon-lock"></span>' +
                                '</div>';
                }
                else {
                    lockIcon = '<div class="member">' +
                                '<span class="member-initials">B</span>' +
                               '</div>';
                }

                if (userStoryData.NumeroActividadTotal > 0) {
                    activityBadge = '<div class="badge">' +
                                        '<span class="badge-icon icon-sm icon-checklist"></span>' +
                                        '<span class="badge-text">' + userStoryData.NumeroActividadTerminada + '/' + userStoryData.NumeroActividadTotal + '</span>' +
                                    '</div>';
                }
                
                if (userStoryData.Color != null) {

                    for (var i = 0; i < 5; i++) {
                        if (i == userStoryData.Color)
                        {
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

                var userStoryBadges = '<div class="badges">' + lockBadge + activityBadge + '</div>';

                var userStoryString = '<div class="list-card">' +
                                            '<div class="list-card-labels clearfix">' +
                                               colorString +
                                            '</div>' +
                                            '<div class="list-card-details clearfix" user-story-id="' + userStoryData.UserStoryId + '">' +
                                                '<a class="list-card-title clear js-card-name" href="#">' +
                                                    '<span class="card-short-id hide">1</span>' + userStoryData.Codigo + '</a>' +
                                                userStoryBadges +
                                                '<div class="list-card-members">' +
                                                    '<div class="member js-validate-click">' +
                                                        '<span class="member-initials">👍</span>' +
                                                    '</div>' +
                                                    lockIcon +
                                                '</div>' +
                                            '</div>' +
                                        '</div>'

                userStoryContainer.append(userStoryString);

            });


            base.Eventos.OnClickCard();
            base.Eventos.OnClickValidate();
        },

        LoadUserStoryWindow: function (userStoryId) {
            $.get('/TaskBoard/UserStoryDetail/' + userStoryId, base.Eventos.OnUserStoryDetailSuccess);
        }
    }

    base.Eventos = {

        OnBoardDataLoad: function (data) {

            base.Funciones.BuildColumns(data);

        },

        OnClickCard: function () {
            $("div.list-cards div.list-card").on("click", "div.list-card-details", function (e) {
                e.stopPropagation();
                var userStoryId = $(this).attr('user-story-id');

                base.Funciones.LoadUserStoryWindow(userStoryId);
            });
        },

        OnClickValidate: function () {

            $("div.list-cards div.list-card div.list-card-details div.list-card-members").on("click", "div.js-validate-click", function (e) {
                e.stopPropagation();

                var userStoryId = $(this).attr('user-story-id');

                base.Controles.ValidateWindowManager.show();

                
            });
            
        },

        OnUserStoryDetailSuccess: function (data) {
            base.Controles.WindowManager.show(data);
        }
    }

    base.init = function () {
        console.log('Init BoardController');
        base.Controles.WindowManager.init();
        base.Controles.ValidateWindowManager.init();
        base.Funciones.LoadBoardData();
    };

}

$(document).ready(function () {
    var controlador = new BoardController();
    controlador.init();
});