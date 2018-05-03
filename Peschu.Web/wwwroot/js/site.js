(function () {
    $(document).ready(function () {
        $('#tags-filter').keyup(function () {
            searchTags($('#tags-filter').val().toLowerCase());
        });
    });

    function searchTags(searchTerm) {
        if (searchTerm.length <= 0) {
            $('.tags-component .tags-list').show();
            return;
        }

        $('.tags-component .tags-list').each(function (index, element) {
            var tag = $(element).text();
            if (tag.toLowerCase().indexOf(searchTerm) < 0) {
                $(element).hide();
            } else {
                $(element).show();
            }
        });
    };
})(); 