
function BoardController() {

    var base = this;

    base.Controles = {
        WindowManager: new BoardWindowManager()
    }

    base.Funciones = {
        LoadUserStoryWindow: function () {
            $.get('/TaskBoard/UserStoryDetail', base.Eventos.OnUserStoryDetailSuccess);
        }
    }

    base.Eventos = {
        OnClickCard: function () {
            $("div.list-cards div.list-card").on("click", "div.list-card-details", function () {
                base.Funciones.LoadUserStoryWindow();
            });
        },
        OnUserStoryDetailSuccess: function (data)
        {
            base.Controles.WindowManager.show(data);
        }
    }

    base.init = function () {
        console.log('Init BoardController');
        base.Controles.WindowManager.init();
        base.Eventos.OnClickCard();
    };

}

$(document).ready(function () {
    var controlador = new BoardController();
    controlador.init();
});