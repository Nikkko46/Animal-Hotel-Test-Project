var scrollPosition;
            
        function pageLoad() {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_beginRequest(beginRequest);
            prm.add_endRequest(endRequest);
        }
        
        function beginRequest() {
            scrollPosition = $('html,body').scrollTop();
        }
        
        function endRequest() {
            $('html,body').scrollTop(scrollPosition);
        }

        function validateDates() {
            const checkIn = document.getElementById('txtCheckIn');
            const checkOut = document.getElementById('txtCheckOut');
            const today = new Date();
            today.setHours(0, 0, 0, 0);

            let isValid = true;
            
            checkIn.classList.remove('invalid-date');
            checkOut.classList.remove('invalid-date');
            
            document.querySelectorAll('.date-error').forEach(el => el.remove());

            if (!checkIn.value || !checkOut.value) {
                if (!checkIn.value) {
                    showError(checkIn, 'Please select a check-in date');
                    isValid = false;
                }
                if (!checkOut.value) {
                    showError(checkOut, 'Please select a check-out date');
                    isValid = false;
                }
                return false;
            }

            const checkInDate = new Date(checkIn.value);
            const checkOutDate = new Date(checkOut.value);

            if (checkInDate < today) {
                showError(checkIn, 'Check-in date cannot be in the past');
                isValid = false;
            }

            if (checkOutDate <= checkInDate) {
                showError(checkOut, 'Check-out date must be after check-in date');
                isValid = false;
            }

            return isValid;
        }

        function showError(element, message) {
            element.classList.add('invalid-date');
            const errorDiv = document.createElement('div');
            errorDiv.className = 'date-error';
            errorDiv.textContent = message;
            element.parentNode.appendChild(errorDiv);
        }

        document.getElementById('txtCheckIn').addEventListener('change', validateDates);
        document.getElementById('txtCheckOut').addEventListener('change', validateDates);