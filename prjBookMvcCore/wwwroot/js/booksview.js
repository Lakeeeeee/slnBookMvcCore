/* 排序判斷動作*/
$(document).ready(function () {
        $("#sort-Select").change(function () {
            var sortValue = $(this).val();
            var $books = $(".book-item");
            if (sortValue === "PublicationDate") {
                $books.sort(function (a, b) {
                    return new Date($(a).data("publication-date")) < new Date($(b).data("publication-date")) ? 1 : -1;
                });
            } else if (sortValue === "discountedPrice") {
                $books.sort(function (a, b) {
                    return $(a).data("price") > $(b).data("price") ? 1 : -1;
                });
            }
            $("#booksview").empty();
            $.each($books, function (index, value) {
                $("#booksview").append(value);
            });
        });
    });
