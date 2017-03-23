var student =
    {
        //Add: function () {
        //    $('.add-student')

        //    $.ajax({
        //        url: '/Admin/Home/Add',
        //        success: function (data) {
        //        },
        //        error: function (data) {
        //        },
        //        type: 'GET'
        //    });
        //},

        Init: function () {
            //console.log('test');
            $('.add-student').on('click', function () {
                $.ajax({
                    url: '/Admin/Home/Add',
                    success: function (data) {
                    },
                    error: function (data) {
                    },
                    type: 'GET'
                });
            })
        }
    };

$(document).ready(student.Init);