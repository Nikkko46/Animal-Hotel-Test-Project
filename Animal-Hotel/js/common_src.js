const animals = ['🐱', '🐕', '🐦', '🐠', '🐹', '🐰', '🦜', '🐢', '🦎'];
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

document.addEventListener('DOMContentLoaded', function() {
    const menuToggle = document.getElementById('menuToggle');
    const navMenu = document.querySelector('.nav-menu');

    menuToggle.addEventListener('click', function() {
        navMenu.style.display = navMenu.style.display === 'flex' ? 'none' : 'flex';
    });

    window.addEventListener('resize', function() {
        if (window.innerWidth >= 768) {
            navMenu.style.display = 'flex';
        } else {
            navMenu.style.display = 'none';
        }
    });
});