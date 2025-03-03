const animals = ['🐹', '🐹', '🐇', '🦔', '🦦', '🐸', '🦎', '🐢', '🕷️', '🦂'];
const bgDecorations = document.querySelector('.bg-decorations');

function createRandomAnimal() {
    const animal = document.createElement('div');
    animal.className = 'animal';
    animal.style.left = Math.random() * 100 + 'vw';
    animal.style.top = Math.random() * 100 + 'vh';
    animal.textContent = animals[Math.floor(Math.random() * animals.length)];
    animal.style.animationDelay = Math.random() * 2 + 's';
    bgDecorations.appendChild(animal);

    setTimeout(() => {
        animal.remove();
    }, 6000);
}

setInterval(createRandomAnimal, 2000);

function toggleDetails(event) {
    event.preventDefault();
    const details = event.target.closest('.breed-info').querySelector('.breed-details');
    details.classList.toggle('active');
    const icon = event.target.querySelector('i');
    icon.classList.toggle('fa-chevron-down');
    icon.classList.toggle('fa-chevron-up');
}

function changeEmoji(event, emoji) {
    event.preventDefault();
    if (!animals.includes(emoji)) {
        animals.push(emoji);
    }
}

function addToDashboard(event, breedName) {
    event.preventDefault();
    sessionStorage.setItem('selectedPetBreed', breedName);
    sessionStorage.setItem('selectedPetType', 'Other');
    window.location.href = 'CreatePetProfile.aspx';
}

function scrollToType() {
    const selectedType = document.getElementById('typeSelect').value;
    if (selectedType) {
        const section = document.getElementById(selectedSize + '-section');
        if (section) {
            section.scrollIntoView({ behavior: 'smooth' });
        }
    }
}

document.addEventListener('DOMContentLoaded', function() {
    let lastScrollTop = 0;
    const sizeSelector = document.querySelector('.type-selector');

    window.addEventListener('scroll', function() {
        if (window.innerWidth <= 768) {
            const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
            
            if (scrollTop > lastScrollTop) {
                sizeSelector.style.transform = 'translateY(100%)';
            } else {
                sizeSelector.style.transform = 'translateY(0)';
            }
            
            lastScrollTop = scrollTop;
        }
    });

    const breedCards = document.querySelectorAll('.breed-card');
    breedCards.forEach(card => {
        let touchStartY;
        
        card.addEventListener('touchstart', (e) => {
            touchStartY = e.touches[0].clientY;
        });

        card.addEventListener('touchmove', (e) => {
            const touchY = e.touches[0].clientY;
            const details = card.querySelector('.breed-details');
            
            if (touchStartY - touchY > 50) {
                details.classList.add('active');
            } else if (touchY - touchStartY > 50) {
                details.classList.remove('active');
            }
        });
    });
});