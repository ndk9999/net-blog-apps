import React, { useEffect, useState } from 'react'
import Tag from './Tag';

const tagData = [
  {
    title: 'Sample title',
    description: 'Put your content here'
  },
  {
    title: 'Microsoft',
    description: 'Microsoft company & technologies'
  },
  {
    title: 'React',
    description: 'Put your content here'
  },
  {
    title: 'MongoDB',
    description: <>Including <b>HTML tag</b> and <span className='text-danger'>styled element</span></>
  },
  {
    title: 'ASP.NET MVC',
    description: ''
  },
];



const TagList = ({ searchTerm }) => {
  const [tags, setTags] = useState(tagData);
  const isEmptyList = tags.length === 0;

  useEffect(() => {
      const filteredTags = searchTerm.trim() !== ''
        ? tagData.filter(t => t.title.includes(searchTerm))
        : tagData;

      setTags(filteredTags);
  }, [searchTerm]);

  return (
    <div>
      <h4>Currently searching for {searchTerm}</h4>

      {isEmptyList ? (
        <div className='text-danger'>
          Could not find any tags
        </div>
      ) : (
        <ul className='tags-list'>
          {tags.map((t, i) => (
            <Tag key={i} id={i + 1} title={t.title}>
              {t.description}
            </Tag>
          ))}
        </ul>
      )}
      
    </div>
  )
}

export default TagList;