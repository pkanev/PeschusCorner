(function () {
    $(document).ready(function () {
        $('#tags-filter').keyup(function () {
            searchTags($('#tags-filter').val().toLowerCase());
        });
    });

    function searchTags(searchTerm) {
        $('.tags-list').each(function (index, element) {
            var tag = $(element).text();
            if (!tag.toLowerCase().includes(searchTerm)) {
                $(element).hide();
            } else {
                $(element).show();
            }
        });
    };
})(); 