displayPopupMsg = (msg) => {
    const popup = $('#todopopup')

    if (popup.not(':hidden')) {
        popup.stop()
        popup.slideUp(400, () => {
            popup.html(msg)
        })
    } else {
        popup.html(msg)
    }

    popup.slideDown(600).delay(3000).slideUp(400)
}

loadTodoList = (hoveredTodoId) => {
    $.ajax({
        url: getActionUrl('TodoListPartial'),
        type: 'POST',
        data: {
            hoveredTodoId: hoveredTodoId
        }
    }).done((data) => {
        $('#todolist-partial').html(data)
    }).fail((e) => {
        displayPopupMsg(e.status + ': ' + e.responseText)
    })
}

loadSchedule = (hoveredTodoId) => {
    $.ajax({
        url: getActionUrl('SchedulePartial'),
        type: 'POST',
        data: {
            hoveredTodoId: hoveredTodoId
        }
    }).done((data) => {
        $('#schedule-partial').html(data)
    }).fail((e) => {
        displayPopupMsg(e.status + ': ' + e.responseText)
    })
}

handleBtnDoneClick = (elem) => {
    var id = $(elem).attr('id')

    $.ajax({
        url: getActionUrl('ChangeDoneStateAjax'),
        data: {
            id: id
        },
        type: 'POST'
    }).done(() => { loadTodoList(id) })
}

handleBtnSubmitClick = (elem) => {
    $(elem).removeClass('animate')
    setTimeout(() => { $(elem).addClass('animate') }, 1)
}

handleFormSubmit = (e) => {
    e.preventDefault()
    var input = $('#desc-input')

    $.ajax({
        url: '/',
        type: 'POST',
        data: {
            Description: input.val(),
            DayNumber: getDayNumber()
        }
    }).done((e) => {
        loadTodoList()
        input.val('')
        displayPopupMsg('new todo added')
    }).fail((e) => {
        displayPopupMsg(Object.values(e.responseJSON))
    })
}


$(document).ready(() => {
    $('#todoadd-form').submit((e) => handleFormSubmit(e))

    loadTodoList()
    loadSchedule()
})
