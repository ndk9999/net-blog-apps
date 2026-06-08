import React from 'react'

const MenuItem = ({link, title}) => {
  return (
    <li className="nav-item">
        <a className="nav-link" href={link}>
            {title}
        </a>
    </li>
  )
}

export default MenuItem;