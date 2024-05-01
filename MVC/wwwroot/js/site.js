// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const container = document.querySelector(".container");
const seats = document.querySelectorAll(".row-seat .seat:not(.sold)");
const count = document.getElementById("count");
const total = document.getElementById("total");

populateUI();

let ticketPrice = +12;

function updateSelectedCount() {
    const selectedSeats = document.querySelectorAll(".row-seat .seat.selected");

    const seatsIndex = [...selectedSeats].map((seat) => [...seats].indexOf(seat));

    document.getElementById("additionalInfo1").value = additionalInfo1;
    document.getElementById("additionalInfo2").value = additionalInfo2;

    localStorage.setItem("selectedSeats", JSON.stringify(seatsIndex));
    localStorage.setItem
    const selectedSeatsCount = selectedSeats.length;

    count.innerText = selectedSeatsCount;
    total.innerText = selectedSeatsCount * ticketPrice;

    var priceField = document.getElementById("priceId");
    var value = (selectedSeatsCount * ticketPrice).toString();
    priceField.value = value;
}


function populateUI() {
    const selectedSeats = JSON.parse(localStorage.getItem("selectedSeats"));

    if (selectedSeats !== null && selectedSeats.length > 0) {
        seats.forEach((seat, index) => {
            if (selectedSeats.indexOf(index) > -1) {
                console.log(seat.classList.add("selected"));
            }
        });
    }
}
console.log(populateUI())
container.addEventListener("click", (e) => {
    if (
        e.target.classList.contains("seat") &&
        !e.target.classList.contains("sold")
    ) {
        e.target.classList.toggle("selected");

        updateSelectedCount();
    }
});

updateSelectedCount();