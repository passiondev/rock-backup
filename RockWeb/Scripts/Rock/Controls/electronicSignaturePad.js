(function ($) {
    'use strict';
    window.Rock = window.Rock || {};
    Rock.controls = Rock.controls || {};

    Rock.controls.electronicSignaturePad = (function () {
        var exports = {
            /**
             * 
             * @param {any} options
             */
            initialize: function (options) {
                if (!options.controlId) {
                    throw 'id is required';
                }

                var self = this;

                /* These all work
                    image/jpeg
                    image/png
                    image/svg+xml
                */

                self.ImageMimeType = options.imageMimeType ?? "image/png";

                var $control = $('#' + options.controlId);

                if ($control.length == 0) {
                    return;
                }

                self.$signaturePadCanvas = $('.js-signature-pad-canvas', $control);
                self.$signatureEntryDrawn = $('.js-signature-entry-drawn', $control);
                self.$signatureEmptyAlert = $('.js-signature-empty-alert', $control);
                self.$clearSignature = $('.js-clear-signature', $control);
                self.$saveSignature = $('.js-save-signature', $control);

                self.$signatureImageDataURL = $('.js-signature-data', $control);

                self.initializeSignaturePad();
            },

            /** */
            initializeSignaturePad: function () {
                var self = this;
                if (!self.$signaturePadCanvas.length) {
                    return
                }

                var signaturePadOptions = {};
                if (self.ImageMimeType == "image/jpeg") {
                    // NOTE That if we use jpeg, we'll have to deal with this https://github.com/szimek/signature_pad/issues/584
                    signaturePadOptions = {
                        penColor: 'black',
                        backgroundColor: 'white'
                    }
                };

                var signaturePad = new SignaturePad(self.$signaturePadCanvas[ 0 ], signaturePadOptions);

                self.$signaturePadCanvas.data('signatureComponent', signaturePad);

                self.$clearSignature.click(function () {
                    signaturePad.clear();
                })

                self.$saveSignature.click(function () {
                    if (signaturePad.isEmpty()) {
                        self.$signatureEmptyAlert.show();
                        return false;
                    }

                    var signatureImageDataUrl = signaturePad.toDataURL(self.ImageMimeType);
                    self.$signatureImageDataURL.val(signatureImageDataUrl);
                });

                window.addEventListener("resize", self.resizeSignatureCanvas);

                signaturePad.addEventListener("beginStroke", () => {
                    // if there was an error showing, hide the error if they start signing again
                    self.$signatureEmptyAlert.show().hide();
                }, { once: true });

                self.resizeSignatureCanvas();
            },

            /** */
            resizeSignatureCanvas: function () {
                var self = this;

                // If the window is resized, that'll affect the drawing canvas
                // also, if there is an existing signature, it'll get messed up, so clear it and
                // make them sign it again. See additional details why 
                // https://github.com/szimek/signature_pad
                var signaturePadCanvas = self.$signaturePadCanvas[ 0 ];
                var containerWidth = self.$signatureEntryDrawn.width();
                if (!containerWidth || containerWidth == 0) {
                    containerWidth = 400;
                }

                const ratio = Math.max(window.devicePixelRatio || 1, 1);
                signaturePadCanvas.width = containerWidth * ratio;
                signaturePadCanvas.height = 100 * ratio;
                signaturePadCanvas.getContext("2d").scale(ratio, ratio);

                var signaturePad = self.$signaturePadCanvas.data('signatureComponent');
                signaturePad.clear();
            }
        };

        return exports;
    }());
}(jQuery));
