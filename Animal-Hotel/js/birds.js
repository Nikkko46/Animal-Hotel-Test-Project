const animals = ['🕊️', '🐦', '🦜', '🐤', '🦢', '🦆', '🦉'];
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
    sessionStorage.setItem('selectedPetType', 'Bird');
    window.location.href = 'CreatePetProfile.aspx';
}

function scrollToSize() {
    const selectedSize = document.getElementById('sizeSelect').value;
    if (selectedSize) {
        const section = document.getElementById(selectedSize + '-section');
        if (section) {
            section.scrollIntoView({ behavior: 'smooth' });
        }
    }
}