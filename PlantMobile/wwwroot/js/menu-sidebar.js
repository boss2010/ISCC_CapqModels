/* Navigation menu: accordion behaviour and the mobile slide-in panel. */
(function ($) {
    'use strict';

    $(function () {
        var $menu = $('#pq-menu .mainNav');
        var $sidebar = $('#pq-sidebar');
        var $backdrop = $('#pq-backdrop');
        var $toggle = $('#pq-menu-toggle');

        function setSidebar(open) {
            $sidebar.toggleClass('open', open);
            $backdrop.toggleClass('open', open);
            $toggle.attr('aria-expanded', open ? 'true' : 'false');
        }

        $toggle.on('click', function () {
            setSidebar(!$sidebar.hasClass('open'));
        });

        $backdrop.on('click', function () {
            setSidebar(false);
        });

        $(document).on('keydown', function (e) {
            if (e.key === 'Escape' || e.keyCode === 27) {
                setSidebar(false);
            }
        });

        $menu.on('click', 'a', function (e) {
            var $link = $(this);
            var $sub = $link.next('ul');

            if (!$sub.length) {
                return; // a real screen link: let the browser navigate
            }

            e.preventDefault();
            var isOpen = $sub.is(':visible');

            // One open branch per level, like the legacy multiAccordion plugin.
            $link.parent().siblings().children('ul:visible').slideUp(400);
            $link.parent().siblings().children('a').removeClass('open');

            if (isOpen) {
                $sub.slideUp(400);
            } else {
                $sub.slideDown(400);
            }

            $link.toggleClass('open', !isOpen);
        });
    });
})(jQuery);
