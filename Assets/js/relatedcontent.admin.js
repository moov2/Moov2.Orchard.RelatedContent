$(document).ready(function() {
    var $relatedContentAddButton = $('.js-add-related-content');
    $relatedContentForm = $('.js-related-content-form'),
        $relatedContentContentSection = $('.js-related-content-content'),
        $relatedContentUrlSection = $('.js-related-content-url-section'),
        $relatedContentContentId = $('.js-related-content-item-id'),
        $relatedContentContentDisplayLink = $('.js-related-content-item-display-link'),
        $relatedContentContentTitle = $('.js-related-content-selected-content'),
        $relatedContentText = $('.js-related-content-text'),
        $relatedContentTypes = $('.js-related-content-types'),
        $relatedContentSaveButton = $('.js-related-content-save-button'),
        $relatedContentJson = $('#RelatedContent_RelatedContentJson'),
        $relatedContentsList = $('.js-related-contents-links-list'),
        $relatedContentsListContainer = $('.js-related-contents-links-list-container'),
        $relatedContentUnpublishedContent = $('.js-related-content-unpublished-content'),
        currentContentLinks = [],
        mode = 'new',
        editIndex = 0;

    /**
     * Returns the id of the selected content item.
     */
    var getRelatedContentContentItemId = function() {
        return ($relatedContentContentSection.is(':visible')) ? $relatedContentContentId.val() : 0;
    };

    /**
     * Returns the text for the related link.
     */
    var getRelatedContentText = function() {
        return $relatedContentText.val().trim();
    };

    /**
     * Validates the related content item entry and enabled / disables
     * the save button.
     */
    var validate = function() {
        var valid = true;

        // user hasn't entered any text for the content item.
        if ($relatedContentText.val().trim() === '') {
            valid = false;
        }

        // user hasn't selected a content item.
        if ($relatedContentContentId.val() === '-1') {
            valid = false;
        }

        // user has chosen an unpublished content item.
        if ($relatedContentUnpublishedContent.is(':visible') === true) {
            valid = false;
        }

        (valid === true) ? $relatedContentSaveButton.removeClass('disabled'): $relatedContentSaveButton.addClass('disabled');
    };

    /**
     * Resets the form.
     */
    var resetForm = function() {
        $relatedContentForm.hide();
        $relatedContentText.val('');
        $relatedContentContentId.val('-1');
        $relatedContentContentDisplayLink.val('');
        $relatedContentAddButton.show();
        $relatedContentContentTitle.html('');
        $relatedContentUnpublishedContent.hide();
        validate();
    };

    resetForm();
    try {
        currentContentLinks = JSON.parse($relatedContentJson.val());
    } catch (error) {
        currentContentLinks = [];
    }

    // shows the form when the user clicks the add button.
    $relatedContentAddButton.on('click', function() {
        mode = 'new';
        $relatedContentForm.show();
        $relatedContentAddButton.hide();
    });

    // resets the form when the user clicks the cancel button.
    $('.js-related-content-cancel-button').on('click', function() {
        resetForm();
        resetEdit();
    });

    // user wishes to select the related content item.
    $('.js-related-content-select-content-btn').on('click', function() {
        $relatedContentContentId.trigger("orchard-admin-contentpicker-open", {
            callback: function(data) {
                $relatedContentContentId.val(data.id);
                $relatedContentContentTitle.html(data.displayText);
                if (typeof(data.displayLink) !== 'undefined') {
                    var $link = $(data.displayLink);
                    if ($link.length > 0) {
                        data.displayLink = $link.attr('target', '_blank')[0].outerHTML;
                    }
                }
                $relatedContentContentDisplayLink.val(data.displayLink);
                $relatedContentText.val(data.displayText);

                (data.displayLink.indexOf('Contents/Item/Display/' + data.id) >= 0) ? $relatedContentUnpublishedContent.show(): $relatedContentUnpublishedContent.hide();

                validate();
            },
            types: $relatedContentTypes.val(),
            baseUrl: '/'
        });
    });

    // user filling out the form.
    $('.js-related-content-text').on('keyup', validate);

    // user attempting to save a related link item.
    $relatedContentSaveButton.on('click', function() {
        // entered related link item is currently invalid.
        if ($relatedContentSaveButton.hasClass('disabled')) {
            return;
        }
        var newItem = {};
        if (mode === 'new') {
            newItem = {
                ContentItemId: getRelatedContentContentItemId(),
                Label: getRelatedContentText()
            };

            // adds the new link to the array of current related links.
            currentContentLinks.push(newItem);

            // adds the new link to the list of the currently related links shown to the user.
            var html = '<li class="last"><div class="summary"><div class="properties"><h3>' + newItem.Label + '</h3> - ';
            html += (newItem.ContentItemId !== 0) ? $relatedContentContentDisplayLink.val() : '<a href=' + newItem.Url + ' target="_blank">' + newItem.Url + '</a>';
            html += '</div><div class="related"><a href="#" data-index="' + (currentContentLinks.length - 1) + '" class="js-edit-related-content">Edit</a> | <a href="#" data-index="' + (currentContentLinks.length - 1) + '" class="js-remove-related-content">Delete</a></div></div></li>';

            $relatedContentsList.children().removeClass('last');
            $relatedContentsList.append(html);
        } else {
            //update existing element and show
            // finds link in the list.
            var $item = $relatedContentsList.find('[data-index=' + editIndex + ']').parents('li');

            // finds link in the current items array.
            var itemIndex = -1;
            for (var i = 0; i < currentContentLinks.length; i++) {
                if (currentContentLinks[i].Label === $item.find('h3').text()) {
                    itemIndex = i;
                }
            }

            currentContentLinks[itemIndex].ContentItemId = getRelatedContentContentItemId();
            currentContentLinks[itemIndex].Label = getRelatedContentText();

            var $properties = $item.find('.properties');
            $properties.find('h3').text(currentContentLinks[itemIndex].Label);
            $properties.find('a').remove();
            $properties.append($relatedContentContentDisplayLink.val());
            resetEdit();
        }

        // updates the JSON that will be passed to the server.
        $relatedContentJson.val(JSON.stringify(currentContentLinks));

        $relatedContentsListContainer.show();

        $('.js-remove-related-content').on('click', removeLink);
        $('.js-edit-related-content').on('click', editLink);

        resetForm();
    });

    /**
     * Removes an existing related link.
     */
    var removeLink = function() {
        var index = $(this).attr('data-index');

        // removes link from the list.
        var $item = $relatedContentsList.find('[data-index=' + index + ']').parents('li');

        // removes link from the current items array.
        var itemIndex = -1;
        for (var i = 0; i < currentContentLinks.length; i++) {
            if (currentContentLinks[i].Label === $item.find('h3').text()) {
                itemIndex = i;
            }
        }

        if (itemIndex > -1) {
            currentContentLinks.splice(itemIndex, 1);
        }

        $item.remove();

        // updates the JSON that will be passed to the server.
        $relatedContentJson.val(JSON.stringify(currentContentLinks));

        // hide the links list if there aren't any.
        if (currentContentLinks.length === 0) {
            $relatedContentsListContainer.hide();
        }

        // ensure the hyperlink doesn't trigger.
        return false;
    }

    /**
     * Edits an existing related link.
     */
    var editLink = function() {
        resetEdit();
        mode = 'edit';
        var index = $(this).attr('data-index');

        // finds link in the list.
        var $item = $relatedContentsList.find('[data-index=' + index + ']').parents('li');

        // finds link in the current items array.
        var itemIndex = -1;
        for (var i = 0; i < currentContentLinks.length; i++) {
            if (currentContentLinks[i].Label === $item.find('h3').text()) {
                itemIndex = i;
            }
        }

        $item.hide();

        editIndex = itemIndex;

        // updates the ui to display edit for this item
        $relatedContentForm.show();
        $relatedContentAddButton.hide();
        var $properties = $item.find('.properties');
        $relatedContentText.val($properties.find('h3').text());
        $relatedContentContentId.val(currentContentLinks[itemIndex].ContentItemId);
        $relatedContentContentTitle.text($properties.find('a').text());
        $relatedContentContentDisplayLink.val($properties.find('a')[0].outerHTML);
        validate();

        // ensure the hyperlink doesn't trigger.
        return false;
    };

    var resetEdit = function() {
        $('.js-related-contents-links-list > li').show();
    };

    // removing a current related link.
    $('.js-remove-related-content').on('click', removeLink);
    // editing a current related link.
    $('.js-edit-related-content').on('click', editLink);
});