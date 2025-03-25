// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    const orderForm = document.getElementById("orderForm");
    const successMessage = document.getElementById("successMessage");

    orderForm.addEventListener("submit", function (event) {
        event.preventDefault();
        
        const formData = new FormData(orderForm);

        fetch(orderForm.action, {
            method: 'POST',
            body: formData
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                console.log("Заявка відправлена:");
                console.log("ПІБ:", formData.get("fullName"));
                console.log("Номер телефону:", formData.get("phoneNumber"));
                console.log("Email:", formData.get("email"));

                orderForm.reset();
                successMessage.style.display = "block";
            }
        });
    });

    function getCookie(name) {
        let cookies = document.cookie.split('; ');
        for (let cookie of cookies) {
            let [key, value] = cookie.split('=');
            if (key === name) {
                return decodeURIComponent(value);
            }
        }
        return null;
    }

    let savedLang = getCookie("Leng");
    if (savedLang) {
        $("#lengSelect").val(savedLang);
    }

    $("#lengSelect").on("change", function () {
        let selectedLang = $(this).val();
        fetch(`/set-leng/${selectedLang}`).then(() => { location.reload() })
            .catch(e => console.error(e));
    });
});

