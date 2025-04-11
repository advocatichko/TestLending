// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    const orderForm = document.getElementById("orderForm");
    const successMessage = document.getElementById("successMessage");

    if (orderForm) {
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
    }

    // Відкладене завантаження YouTube відео
    const youtubeContainers = document.querySelectorAll('.youtube-video-container');
    
    if (youtubeContainers.length > 0) {
        console.log("Знайдено", youtubeContainers.length, "YouTube контейнери");
        
        youtubeContainers.forEach(container => {
            container.addEventListener('click', function() {
                const videoId = this.getAttribute('data-video-id');
                const videoTitle = this.getAttribute('data-video-title');
                
                console.log("Клік по відео:", videoId, videoTitle);
                
                // Створюємо iframe з відео YouTube
                const iframe = document.createElement('iframe');
                iframe.setAttribute('width', '100%');
                iframe.setAttribute('height', '100%');
                iframe.setAttribute('src', `https://www.youtube.com/embed/${videoId}?autoplay=1&rel=0`);
                iframe.setAttribute('title', videoTitle);
                iframe.setAttribute('frameborder', '0');
                iframe.setAttribute('allow', 'accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share');
                iframe.setAttribute('allowfullscreen', '');
                
                // Замінюємо фасад на iframe
                this.innerHTML = '';
                this.appendChild(iframe);
                
                // Видаляємо обробник кліку, щоб не додавати iframe повторно
                this.removeEventListener('click', arguments.callee);
            });
        });
    } else {
        console.warn("YouTube контейнери не знайдено на сторінці");
    }
});

// JavaScript для відтворення YouTube відео
document.addEventListener('DOMContentLoaded', function() {
    const videoContainers = document.querySelectorAll('.youtube-video-container');
    
    videoContainers.forEach(container => {
        const videoId = container.getAttribute('data-video-id');
        if (!videoId) return;
        
        // Створюємо превʼю з зображенням
        const thumbnailUrl = `https://img.youtube.com/vi/${videoId}/maxresdefault.jpg`;
        const playButton = document.createElement('div');
        playButton.className = 'play-button';
        
        // Додаємо зображення та кнопку відтворення
        container.style.backgroundImage = `url(${thumbnailUrl})`;
        container.style.backgroundSize = 'cover';
        container.style.backgroundPosition = 'center';
        container.appendChild(playButton);
        
        // Додаємо обробник кліку
        container.addEventListener('click', function() {
            const iframe = document.createElement('iframe');
            iframe.setAttribute('allowfullscreen', '');
            iframe.setAttribute('allow', 'autoplay');
            iframe.setAttribute('src', `https://www.youtube.com/embed/${videoId}?rel=0&showinfo=0&autoplay=1`);
            
            // Очищаємо контейнер і додаємо iframe
            container.innerHTML = '';
            container.appendChild(iframe);
        });
    });
});
