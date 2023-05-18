/* 排序判斷動作*/
function booksview() {
    var sortBy = document.getElementById("sort-Select").value;

    // Get the book list container element
    var bookList = document.getElementById("booksview");

    // Get all the book items inside the book list
    var bookItems = bookList.getElementsByClassName("book-item");

    // Convert the book items into an array
    var bookItemsArray = Array.from(bookItems);

    // Sort the book items based on the selected sorting method
    if (sortBy === "PublicationDate") {
        bookItemsArray.sort(function (a, b) {
            var dateA = new Date(a.getAttribute("data-publication-date"));
            var dateB = new Date(b.getAttribute("data-publication-date"));
            return dateB - dateA;
        });
    } else if (sortBy === "discountedPrice") {
        bookItemsArray.sort(function (a, b) {
            var priceA = parseInt(a.getAttribute("data-price"));
            var priceB = parseInt(b.getAttribute("data-price"));
            return priceA - priceB;
        });
    }

    // Clear the book list
    bookList.innerHTML = "";

    // Append the sorted book items back to the book list
    bookItemsArray.forEach(function (item) {
        bookList.appendChild(item);
    });
}
