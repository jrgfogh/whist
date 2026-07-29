import React, { useState } from "react";

interface EditableTextProps {
    text: string;
    saveEdit: (value: string) => void;
}

export function EditableText(props: EditableTextProps) {
    const [isEditing, setIsEditing] = useState(false);

    const saveEdit = (event: React.FocusEvent<HTMLInputElement> | React.KeyboardEvent<HTMLInputElement>) => {
        props.saveEdit((event.target as HTMLInputElement).value);
        setIsEditing(false);
    };

    const handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
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
                onBlur={saveEdit} />);
    else
        return (<button className="btn w-75 float-left text-start" onClick={() =>
            setIsEditing(true)}>{ props.text }</button>);
}
