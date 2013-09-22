(function ($,es) {
    if (es.activeSpeakerSlug) {
        $('#' + es.activeSpeakerSlug).modal('show');
    }
})(jQuery, window.EventSite || (window.EventSite = {}));