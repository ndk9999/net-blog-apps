import React, { useState } from 'react'
import TagForm from './TagForm';
import TagList from './TagList';

const TagCloud = () => {
    const [tagName, setTagName] = useState('');

    function updateTagName(event) {
        setTagName(event.target.value);
    }

  return (
    <div>
        <TagForm onTagEntered={updateTagName} />
        <TagList searchTerm={tagName} />
    </div>
  )
}

export default TagCloud;