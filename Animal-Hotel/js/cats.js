const animals = ['🐱', '😺', '😸', '😼', '😽', '😾', '🐈', '🐈‍⬛'];
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
    sessionStorage.setItem('selectedPetType', 'Cat');
    window.location.href = 'CreatePetProfile.aspx';
}

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