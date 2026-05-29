(function () {
    function bindPasswordToggle(wrap) {
        var input = wrap.querySelector('.lex-password-input');
        var btn = wrap.querySelector('.lex-password-toggle');
        if (!input || !btn) return;

        var show = function () { input.type = 'text'; };
        var hide = function () { input.type = 'password'; };

        btn.addEventListener('mousedown', function (e) {
            e.preventDefault();
            show();
        });
        btn.addEventListener('mouseup', hide);
        btn.addEventListener('mouseleave', hide);
        btn.addEventListener('touchstart', function (e) {
            e.preventDefault();
            show();
        }, { passive: false });
        btn.addEventListener('touchend', hide);
        btn.addEventListener('touchcancel', hide);
        btn.addEventListener('keydown', function (e) {
            if (e.key === ' ' || e.key === 'Enter') {
                e.preventDefault();
                show();
            }
        });
        btn.addEventListener('keyup', hide);
    }

    document.querySelectorAll('.lex-password-wrap').forEach(bindPasswordToggle);
})();
