//------------------------------------------------------------------------------------------
// SubWindow.js
// ---------------
// Represents a sub window object, that can show a new window that contains some content, over the page.
//------------------------------------------------------------------------------------------

// C'tor Function:
function SubWindow(bgColor, html) {

    // Properties:
    this.bgColor = bgColor;
    this.html = html;
}
SubWindow.prototype.Show = function () {
    var _wrapper = $('<div />').addClass('subWindow-wrapper');
    var _window = $('<div />').addClass('subWindow-window').css("background-color", this.bgColor)
                              .appendTo(_wrapper).animate({ opacity: "toggle" }, 500);
    var _close = $('<button />').addClass('subWindow-close').appendTo(_window).click(function () {
        _window.animate({ height: '+=400px', opacity: "toggle" }, 600, function () {
            _wrapper.animate({ opacity: "toggle" }, 600, function () {
                _wrapper.remove();
            });
        });

    });
    var _content = $('<div />').addClass('subWindow-content').html(this.html).appendTo(_window);

    $('section.actualContent').append(_wrapper);
};

