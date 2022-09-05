var Sticky = {
    count: 0,

    init: function init_TSN(id, deletable) {
        //var id = guid();

        var btnDelete = deletable ? '<span id="delete" class="fal fa-trash"  style="position:absolute; left: 280px; top: -35px; font-size: 15px"></span>' : "";

        var offset = this.count * 20;
        $("body").prepend(
            '<div id="postit" class="Sticky_' + id + '" style="top:' + offset + 'px;left:' + offset + 'px">'
            +
            '<span id="minimise" class="fal fa-times"  style="position:absolute; left: 8px; top: -35px; font-size: 15px"></span>'
            +
            btnDelete
            +
            '<p id="postit-author" style="margin-top:-40px;font-weight:bold"></p>' +
            '<p id="postit-body"></p>'
            +
            '</div>'
        );

        $("#minimise").click(function (e) {
            console.log(e);
            var id = $(e.currentTarget).parent().attr("class");
            $("." + id).fadeOut();
        });

        $("#delete").click(function (e) {
            console.log(e);
            var id = $(e.currentTarget).parent().attr("class");
            Feature_Sticky_Delete(id.split("Sticky_")[1]);
        });

        (function ($) {
            $.fn.drags = function (opt) {

                opt = $.extend({ handle: "", cursor: "move" }, opt);

                if (opt.handle === "") {
                    var $el = this;
                } else {
                    var $el = this.find(opt.handle);
                }

                return $el.css('cursor', opt.cursor).on("mousedown", function (e) {
                    if (opt.handle === "") {
                        var $drag = $(this).addClass('draggable');
                    } else {
                        var $drag = $(this).addClass('active-handle').parent().addClass('draggable');
                    }
                    var z_idx = $drag.css('z-index'),
                        drg_h = $drag.outerHeight(),
                        drg_w = $drag.outerWidth(),
                        pos_y = $drag.offset().top + drg_h - e.pageY,
                        pos_x = $drag.offset().left + drg_w - e.pageX;
                    $drag.parents().on("mousemove", function (e) {
                        $('.draggable').offset({
                            top: e.pageY + pos_y - drg_h,
                            left: e.pageX + pos_x - drg_w
                        }).on("mouseup", function () {
                            $(this).removeClass('draggable').css('z-index', z_idx);
                        });
                    });
                    e.preventDefault(); // disable selection

                }).on("mouseup", function () {

                    if (opt.handle === "") {

                        $(this).removeClass('draggable');

                    } else {

                        $(this).removeClass('active-handle').parent().removeClass('draggable');
                    }

                });

            };
        })(jQuery);

        $('#postit').drags();
        this.count++;

    },

    write: function write(str, author) {
        $("#postit-body").text("");
        $("#postit-body").text(str);
        $("#postit-author").text(author);
    },

    read: function () {
        return $("#postit-body").text();
    }

};

function Tellis_Sticky() {
    return Sticky;
}
