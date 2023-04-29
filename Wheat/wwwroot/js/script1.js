var a = document.getElementsByClassName('inst');

for (var i = 0; i < a.length; i = i + 1) {
    a[i].addEventListener('click', function () {
        this.classList.toggle('active');
        var pnl = this.nextElementSibling;
        if (pnl.style.maxHeight) {
            pnl.style.maxHeight = null;
        }
        else {
            pnl.style.maxHeight = pnl.scrollHeight + 'px';
        }
    });
}