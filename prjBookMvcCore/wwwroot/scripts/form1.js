const form1 = document.getElementById('form1');
const nextForm1 = document.getElementById('nextForm1');

nextForm1.addEventListener('click', async () => {

    const response = fetch('@URL.Action("Action1", "Order")', {
        method: 'POST',
        body: new FormData(form1)
        })



})