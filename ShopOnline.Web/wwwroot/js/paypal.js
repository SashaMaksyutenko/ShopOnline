function initPayPalButton(clientId, clientSecret, currencyCode, userId) {
    //debugger;
    // This is stored just in case the user cancels the other
    // or there is an error in the other process.
    let orderId = null;
    paypal.Buttons({

        // Call your server to set up the transaction
        createOrder: function (data, actions) {
            return fetch(`/api/paypal/checkout/order/create/${clientId}/${clientSecret}/${currencyCode}/${userId}`, {
                method: 'post'
            }).then(function (res) {
                //console.log(res)
                return res.json();
            }).then(function (orderData) {
                //console.log(orderData)
                return orderData.orderID;
            });
        },

        // Call your server to finalize the transaction
        onApprove: function (data, actions) {
            return fetch(`/api/paypal/checkout/order/approved/${data.orderID}`, {
                method: 'post'
            }).then(function (res) {
                return res.json();
            }).then(function (orderData) {
                // Three cases to handle:
                //   (1) Recoverable INSTRUMENT_DECLINED -> call actions.restart()
                //   (2) Other non-recoverable errors -> Show a failure message
                //   (3) Successful transaction -> Show confirmation or thank you

                // This example reads a v2/checkout/orders capture response, propagated from the server
                // You could use a different API or structure for your 'orderData'
                let errorDetail = Array.isArray(orderData.details) && orderData.details[0];

                if (errorDetail && errorDetail.issue === 'INSTRUMENT_DECLINED') {
                    return actions.restart(); // Recoverable state, per:
                    // https://developer.paypal.com/docs/checkout/integration-features/funding-failure/
                }

                if (errorDetail) {
                    let msg = 'Sorry, your transaction could not be processed.';
                    if (errorDetail.description) msg += '\n\n' + errorDetail.description;
                    if (orderData.debug_id) msg += ' (' + orderData.debug_id + ')';
                    return alert(msg); // Show a failure message (try to avoid alerts in production environments)
                }

                // Successful capture! For demo purposes:
                console.log('Capture result', orderData, JSON.stringify(orderData, null, 2));
                let transaction = orderData.purchase_units[0].payments.captures[0];
                alert('Transaction ' + transaction.status + ': ' + transaction.id + '\n\nSee console for all available details');

                // Replace the above to show a success message within this page, e.g.
                // const element = document.getElementById('paypal-button-container');
                // element.innerHTML = '';
                // element.innerHTML = '<h3>Thank you for your payment!</h3>';
                // Or go to another URL:  actions.redirect('thank_you.html');
            });
        },
        // Buyer cancelled the payment
        onCancel: function (data, actions) {
            //debugger;
            fetch(`/api/paypal/checkout/order/cancel/${data.orderID}`);
        },

        // An error occurred during the transaction
        onError: function (err) {
            //debugger;
            fetch(`/api/paypal/checkout/order/error/${orderId}/${encodeURIComponent(err)}`);
        }

    }).render('#paypal-button-container');
}
