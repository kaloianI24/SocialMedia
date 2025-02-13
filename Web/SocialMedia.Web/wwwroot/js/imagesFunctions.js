function initializePostFeatures(post) {
    const carouselImages = post.querySelector('.carousel-images');
    const dots = post.querySelectorAll('.dot');
    const leftArrow = post.querySelector('.left-arrow');
    const rightArrow = post.querySelector('.right-arrow');
    const imageCounter = post.querySelector('.image-counter');

    if (carouselImages && dots.length > 0 && leftArrow && rightArrow && imageCounter) {
        let currentIndex = 0;
        const totalImages = dots.length;

        function updateCarousel() {
            carouselImages.style.transform = `translateX(-${currentIndex * 100}%)`;

            dots.forEach((dot, index) => {
                dot.classList.toggle('active', index === currentIndex);
            });

            imageCounter.textContent = `${currentIndex + 1} / ${totalImages}`;
        }

        rightArrow.addEventListener('click', () => {
            if (currentIndex < totalImages - 1) {
                currentIndex++;
            } else {
                currentIndex = 0;
            }
            updateCarousel();
        });

        leftArrow.addEventListener('click', () => {
            if (currentIndex > 0) {
                currentIndex--;
            } else {
                currentIndex = totalImages - 1;
            }
            updateCarousel();
        });

        dots.forEach((dot, index) => {
            dot.addEventListener('click', () => {
                currentIndex = index;
                updateCarousel();
            });
        });

        updateCarousel();
    }

    const toggleBtn = post.querySelector('.toggle-btn');
    const hiddenText = post.querySelector('.post-description-hidden-text');

    if (toggleBtn && hiddenText) {
        toggleBtn.addEventListener('click', () => {
            if (hiddenText.style.display === 'none' || hiddenText.style.display === '') {
                hiddenText.style.display = 'inline';
                toggleBtn.textContent = 'Read less';
            } else {
                hiddenText.style.display = 'none';
                toggleBtn.textContent = 'Read more';
            }
        });
    }
}

function initializeImages() {
    document.querySelectorAll('.post-image').forEach(image => {
        image.onload = () => {
            const containerAspect = 1;
            const imageAspect = image.naturalWidth / image.naturalHeight;

            if (imageAspect > containerAspect) {
                image.style.width = 'auto';
                image.style.height = '100%';
                image.style.objectFit = 'cover';
            } else {
                image.style.width = '100%';
                image.style.height = 'auto';
                image.style.objectFit = 'cover';
            }
        };
        if (image.complete) {
            image.onload();
        }
    });
}

const observer = new MutationObserver((mutationsList) => {
    for (const mutation of mutationsList) {
        if (mutation.type === 'childList') {
            mutation.addedNodes.forEach(node => {
                if (node.nodeType === 1 && node.classList.contains('post')) {
                    initializePostFeatures(node);
                    initializeImages();
                }
            });
        }
    }
});

observer.observe(document.body, { childList: true, subtree: true });

document.querySelectorAll('.post').forEach(post => {
    initializePostFeatures(post);
    initializeImages();
});