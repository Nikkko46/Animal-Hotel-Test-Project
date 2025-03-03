window.onload = function() {
    const selectedBreed = sessionStorage.getItem('selectedPetBreed');
    const selectedType = sessionStorage.getItem('selectedPetType');
    const selectedOtherType = sessionStorage.getItem('selectedOtherType');
    
    if (selectedBreed && selectedType) {
        document.getElementById('<%= txtPetBreed.ClientID %>').value = selectedBreed;
        document.getElementById('<%= ddlPetType.ClientID %>').value = selectedType;

        if (selectedType === 'Other' && selectedOtherType) {
            document.getElementById('<%= ddlOtherType.ClientID %>').value = selectedOtherType;
            document.getElementById('otherTypeGroup').style.display = 'block';
        }
        
        sessionStorage.removeItem('selectedPetBreed');
        sessionStorage.removeItem('selectedPetType');
        sessionStorage.removeItem('selectedOtherType');
    }

    document.getElementById('<%= ddlPetType.ClientID %>').addEventListener('change', function() {
        const otherTypeGroup = document.getElementById('otherTypeGroup');
        if (this.value === 'Other') {
            otherTypeGroup.style.display = 'block';
        } else {
            otherTypeGroup.style.display = 'none';
            document.getElementById('<%= ddlOtherType.ClientID %>').value = '';
        }
    });
};