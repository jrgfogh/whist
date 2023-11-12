import { useState } from "react";

export function EditableText(props) {
    const [isEditing, setIsEditing] = useState(false);

    const saveEdit = (event) => {
        props.saveEdit(event.target.value);
        setIsEditing(false);
    };

    const handleKeyDown = (event) => {
        if (event.key === "Escape")
            setIsEditing(false);
        if (event.key === "Enter")
            saveEdit(event);
    };

    if (isEditing)
        return (
            <input type="text" className="form-control w-75 float-left"
                defaultValue={props.text} placeholder="Table Name"
                onKeyDown={handleKeyDown}
                onSubmit={saveEdit}
                onBlur={saveEdit} />);
    else
        return (<button className="btn w-75 float-left text-start" onClick={() =>
            setIsEditing(true)}>{ props.text }</button>);
}