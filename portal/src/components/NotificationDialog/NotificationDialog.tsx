import React, { useState, useEffect } from "react";
import { v4 as uuidv4 } from "uuid";
import { Notification } from "../../types/Notification";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  useMediaQuery,
  useTheme,
} from "@material-ui/core";
import { Transition } from "./Transition";

interface Props {
  title: string;
  open: boolean;
  notification?: Notification;
  onCancel: () => void;
  onSave: (notification: Notification) => void;
}

const NotificationDialog: React.FC<Props> = ({
  title: dialogTitle,
  open,
  notification,
  onSave,
  onCancel,
}) => {
  const theme = useTheme();
  const fullScreen = useMediaQuery(theme.breakpoints.down("sm"));
  const [title, setTitle] = useState<string>(notification?.title || "");
  const [body, setBody] = useState<string>(notification?.body || "");

  const handleSave = () => {
    if (!notification) {
      onSave({ id: uuidv4(), title: title, body: body });
    } else {
      onSave({ ...notification!, title: title, body: body });
    }
  };

  useEffect(() => {
    if (open && !notification) {
      setTitle("");
      setBody("");
    } else if (open && !!notification) {
      setTitle(notification.title);
      setBody(notification.body);
    }
  }, [open, notification]);

  return (
    <Dialog
      open={open}
      onClose={onCancel}
      fullScreen={fullScreen}
      TransitionComponent={fullScreen ? Transition : undefined}
      fullWidth
    >
      <DialogTitle>{dialogTitle}</DialogTitle>
      <DialogContent>
        <TextField
          autoFocus
          label="Title"
          type="text"
          fullWidth
          variant="outlined"
          value={title}
          onChange={(event) => setTitle(event.currentTarget.value)}
        />
        <Box marginTop={3}>
          <TextField
            label="Body"
            type="text"
            fullWidth
            variant="outlined"
            multiline
            rows={4}
            value={body}
            onChange={(event) => setBody(event.currentTarget.value)}
          />
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={onCancel}>Cancel</Button>
        <Button onClick={handleSave} color="primary" variant="contained">
          {!notification ? "Add" : "Save"}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default NotificationDialog;
