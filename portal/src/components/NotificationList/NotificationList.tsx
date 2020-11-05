import React, { useState } from "react";
import S from "./NotificationList.style";
import { Typography, Fab, IconButton, Snackbar } from "@material-ui/core";
import { Notification } from "../../types/Notification";
import NotificationItem from "../NotificationItem/NotificationItem";
import AddIcon from "@material-ui/icons/Add";
import NotificationDialog from "../NotificationDialog/NotificationDialog";
import SendIcon from "@material-ui/icons/Send";
import EditIcon from "@material-ui/icons/Edit";
import DeleteIcon from "@material-ui/icons/Delete";
import CloseIcon from "@material-ui/icons/Close";
import { API } from "../../api/Api";

type SnackbarState = {
  open: boolean;
  message: string;
};

const NotificationList = () => {
  const [items, setItems] = useState<Notification[]>([]);
  const [dialogOpen, setDialogOpen] = useState<boolean>(false);
  const [snackbar, setSnackbar] = useState<SnackbarState>({
    open: false,
    message: "",
  });
  const [selectedNotification, setSelectedNotification] = useState<
    Notification | undefined
  >(undefined);

  const onDialogSaved = (notification: Notification) => {
    if (selectedNotification) {
      const newItems = [...items];
      const newNot = newItems.find((x) => x.id === notification.id);
      if (!newNot) return;
      newNot.title = notification.title;
      newNot.body = notification.body;
      setItems(newItems);
      setSelectedNotification(undefined);
    } else {
      setItems((prev) => [notification, ...prev]);
    }
    setDialogOpen(false);
  };

  const onNotificationClick = (notification: Notification) => {
    setSelectedNotification((prev) => {
      if (prev?.id === notification.id) {
        return undefined;
      }
      return notification;
    });
  };

  const onItemDelete = () => {
    if (!selectedNotification) return;
    const index = items.indexOf(selectedNotification);
    const newItems = [...items];
    newItems.splice(index, 1);
    setItems(newItems);
    setSelectedNotification(undefined);
  };

  const onFabClick = () => {
    setSelectedNotification(undefined);
    setDialogOpen(true);
  };

  const onSend = async () => {
    const result = await API.sendNotifications(items);
    if (result) {
      setItems([]);
      setSnackbar({
        open: true,
        message: "Notifications sent!",
      });
    } else {
      setSnackbar({
        open: true,
        message: "Could not send notification, please try again",
      });
    }
  };

  return (
    <>
      <S.Root>
        <S.Header>
          <Typography variant="h6" component="h1">
            Notificaties
          </Typography>
          <S.HeaderButtons>
            {selectedNotification && (
              <>
                <IconButton onClick={onItemDelete}>
                  <DeleteIcon />
                </IconButton>
                <IconButton onClick={() => setDialogOpen(true)}>
                  <EditIcon />
                </IconButton>
              </>
            )}
            <IconButton disabled={items.length === 0} onClick={onSend}>
              <SendIcon />
            </IconButton>
          </S.HeaderButtons>
        </S.Header>
        <S.List>
          {items.map((x, i) => (
            <NotificationItem
              key={x.id}
              notification={x}
              selected={x.id === selectedNotification?.id}
              onClick={() => onNotificationClick(x)}
            />
          ))}
        </S.List>
        <S.FabContainer>
          <Fab color="primary" onClick={onFabClick}>
            <AddIcon />
          </Fab>
        </S.FabContainer>
      </S.Root>
      <NotificationDialog
        open={dialogOpen}
        onCancel={() => setDialogOpen(false)}
        onSave={onDialogSaved}
        notification={selectedNotification}
        title={selectedNotification ? "Edit Notification" : "New Notification"}
      />
      <Snackbar
        anchorOrigin={{
          vertical: "top",
          horizontal: "center",
        }}
        open={snackbar.open}
        autoHideDuration={6000}
        onClose={() => setSnackbar((prev) => ({ ...prev, open: false }))}
        message={snackbar.message}
        action={
          <IconButton
            size="small"
            color="inherit"
            onClick={() => setSnackbar((prev) => ({ ...prev, open: false }))}
          >
            <CloseIcon fontSize="small" />
          </IconButton>
        }
      />
    </>
  );
};

export default NotificationList;
