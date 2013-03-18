$(function () {
    var playListURL = 'http://gdata.youtube.com/feeds/api/users/hibernatingrhinos/uploads?v=2&alt=json&callback=?';
    var videoURL = 'http://www.youtube.com/watch?v=';
    $.getJSON(playListURL, function (data) {
        var list_data = "";
        $.each(data.feed.entry, function (i, item) {
            var feedTitle = item.title.$t;
            var feedURL = item.link[1].href;
            var fragments = feedURL.split("/");
            var videoID = fragments[fragments.length - 2];
            var url = videoURL + videoID;
            var publishedDate = $.localtime.toLocalTime(item.published.$t, 'dd-MM-yyyy HH:mm');
            var thumb = "http://img.youtube.com/vi/" + videoID + "/default.jpg";
            list_data +=
                '<div class="row well">' +
                    '<div class="span8">' +
                    '<p class="lead">' + feedTitle + '</p>' +
                    '<h6 class="muted"><span class="localtime">' + publishedDate + '</span></h6>' +
                    '</div>' +
                    '<div class="span2 offset1">' +
                    '<a class="btn btn-link" href="' + url + '">YouTube</a>' +
                    '</div>' +
                    '</div>';
        });

        $('#youtubeWait').hide();
        $(list_data).insertAfter("#history");
    });
});