const toggleB = document.getElementsByClassName('toggle')[0]; //button
const navbarL = document.getElementsByClassName('links'); //links

toggleB.addEventListener('click', function () {
    for (var j = 0; j < navbarL.length; j++)
        navbarL[j].classList.toggle('active');
});
